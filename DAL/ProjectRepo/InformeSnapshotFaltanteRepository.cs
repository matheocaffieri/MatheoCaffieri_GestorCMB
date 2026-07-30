using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DAL.FactoryDAL;
using DomainModel;
using DAL.DAL_Interfaces;

// Alias a la entidad EF para dejar clara la separación EF / dominio.
using SnapEf = DAL.Informe_snapshot_faltante;

namespace DAL.ProjectRepo
{
    // Los métodos de escritura NO llaman a SaveChanges: la persistencia la dispara el UnitOfWork al Commit.
    public class InformeSnapshotFaltanteRepository : IInformeSnapshotFaltanteRepository
    {
        private readonly GestorCMBEntities _context;
        private readonly DbSet<SnapEf> _set;

        // Proyección EF -> Dominio. El snapshot sólo conserva los campos que se muestran del faltante
        // (los Material_faltante originales ya no existen, así que Id/IdProyecto no aplican).
        private static readonly Expression<Func<SnapEf, DomainModel.MaterialFaltante>> ToDomainExpr =
            s => new DomainModel.MaterialFaltante
            {
                DescripcionArticuloFaltante = s.descripcionArticuloFaltante,
                TipoMaterialFaltante = s.tipoMaterialFaltante,
                TipoUnidadMaterialFaltante = s.tipoUnidadMaterialFaltante,
                CantidadFaltante = s.cantidadFaltante
            };

        public InformeSnapshotFaltanteRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<SnapEf>();
        }

        public void Guardar(Guid idInformeCompra, IEnumerable<MaterialFaltante> materiales)
        {
            if (idInformeCompra == Guid.Empty)
                throw new ArgumentException("idInformeCompra requerido", nameof(idInformeCompra));

            var lista = materiales?.ToList() ?? new List<MaterialFaltante>();

            // Reemplazar cualquier snapshot previo del mismo informe (idempotente).
            var previos = _set.Where(s => s.idInformeCompra == idInformeCompra).ToList();
            if (previos.Count > 0) _set.RemoveRange(previos);

            foreach (var m in lista)
            {
                _set.Add(new SnapEf
                {
                    idSnapshotFaltante = Guid.NewGuid(),
                    idInformeCompra = idInformeCompra,
                    descripcionArticuloFaltante = m.DescripcionArticuloFaltante ?? string.Empty,
                    tipoMaterialFaltante = m.TipoMaterialFaltante ?? string.Empty,
                    tipoUnidadMaterialFaltante = m.TipoUnidadMaterialFaltante ?? string.Empty,
                    cantidadFaltante = m.CantidadFaltante
                });
            }
        }

        public List<MaterialFaltante> Leer(Guid idInformeCompra)
        {
            return _set.AsNoTracking()
                       .Where(s => s.idInformeCompra == idInformeCompra)
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
