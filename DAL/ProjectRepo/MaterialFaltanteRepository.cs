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
using DomainModel.Interfaces;

// Alias a la entidad EF (ajustá el namespace si difiere)
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
                // Si en EF es decimal, casteamos a float para el dominio
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

    }
}
