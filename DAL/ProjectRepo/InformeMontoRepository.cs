using DAL.FactoryDAL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Linq;

namespace DAL.ProjectRepo
{
    public class InformeMontoRepository : IInformeMontoRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;

        public InformeMontoRepository(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
        }

        public void Upsert(InformeMonto informe)
        {
            var existing = _context.Informe_monto.FirstOrDefault(x => x.idProyecto == informe.IdProyecto);

            if (existing == null)
            {
                _context.Informe_monto.Add(new Informe_monto
                {
                    idInformeMonto  = Guid.NewGuid(),
                    idProyecto      = informe.IdProyecto,
                    totalEmpleados  = informe.TotalEmpleados,
                    totalMateriales = informe.TotalMateriales,
                    montoTotal      = informe.MontoTotal
                });
            }
            else
            {
                existing.totalEmpleados  = informe.TotalEmpleados;
                existing.totalMateriales = informe.TotalMateriales;
                existing.montoTotal      = informe.MontoTotal;
            }
        }
    }
}
