using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using DAL.FactoryDAL;

using DomainModel;
using DAL.DAL_Interfaces;

// Alias a la entidad EF para evitar choque de nombres con DomainModel.MaterialFaltante.
using MaterialFaltanteEf = DAL.Material_faltante;

namespace DAL.ProjectRepo
{
    public class MaterialFaltanteRepository : IMaterialesFaltantesRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<MaterialFaltanteEf> _set;

        // Proyección EF -> Dominio 100% traducible a SQL
        private static readonly Expression<Func<MaterialFaltanteEf, DomainModel.MaterialFaltante>> ToDomainExpr =
            d => new DomainModel.MaterialFaltante
            {
                IdMaterialFaltante = d.idMaterialFaltante,
                DescripcionArticuloFaltante = d.descripcionArticuloFaltante,
                TipoMaterialFaltante = d.tipoMaterialFaltante,
                TipoUnidadMaterialFaltante = d.tipoUnidadMaterialFaltante,
                IdProyecto = d.idProyecto,
                CantidadFaltante = (int)d.cantidadFaltante
            };

        public MaterialFaltanteRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<MaterialFaltanteEf>();
        }


        public List<MaterialFaltante> GetAll(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad)
        {
            if (cantidad <= 0) return;

            var row = _set.FirstOrDefault(x =>
                x.idProyecto == idProyecto &&
                x.descripcionArticuloFaltante == descripcion &&
                x.tipoMaterialFaltante == tipoMaterial &&
                x.tipoUnidadMaterialFaltante == tipoUnidad
            );

            if (row == null)
            {
                var nuevo = new MaterialFaltanteEf
                {
                    idMaterialFaltante = Guid.NewGuid(),
                    idProyecto = idProyecto,
                    descripcionArticuloFaltante = descripcion,
                    tipoMaterialFaltante = tipoMaterial,
                    tipoUnidadMaterialFaltante = tipoUnidad,
                    cantidadFaltante = cantidad
                };
                _set.Add(nuevo);
            }
            else
            {
                row.cantidadFaltante += cantidad;
                _context.Entry(row).State = EntityState.Modified;
            }

        }

        public List<Guid> GetIdsByProyecto(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto)
                       .Select(d => d.idMaterialFaltante)
                       .ToList();
        }

        public List<MaterialFaltante> GetByIds(IEnumerable<Guid> ids)
        {
            var idList = ids?.ToList() ?? new List<Guid>();
            if (idList.Count == 0) return new List<MaterialFaltante>();

            return _set.AsNoTracking()
                       .Where(d => idList.Contains(d.idMaterialFaltante))
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public List<MaterialFaltante> GetByIdsAndProyecto(IEnumerable<Guid> ids, Guid idProyecto)
        {
            var idList = ids?.ToList() ?? new List<Guid>();
            if (idList.Count == 0) return new List<MaterialFaltante>();

            return _set.AsNoTracking()
                       .Where(d => idList.Contains(d.idMaterialFaltante) && d.idProyecto == idProyecto)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        // Borra en bloque los faltantes indicados (fetch tracked para que el Commit del UnitOfWork persista el DELETE).
        public void DeleteByIds(IEnumerable<Guid> ids)
        {
            var idList = ids?.ToList() ?? new List<Guid>();
            if (idList.Count == 0) return;

            var rows = _set.Where(d => idList.Contains(d.idMaterialFaltante)).ToList();
            _set.RemoveRange(rows);
        }

    }
}
