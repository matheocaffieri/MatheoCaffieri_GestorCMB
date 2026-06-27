using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using Services.Logs;
using System;
using System.Linq;

using BL.BL_Interfaces;

namespace BL
{
    public class InformeMontoBL : IInformeMontoBL
    {
        /// <summary>
        /// Recalcula totales de empleados y materiales del proyecto,
        /// persiste en Informe_monto (upsert) y devuelve el resultado.
        /// </summary>
        public InformeMonto Recalcular(Guid idProyecto)
        {
            // Obtener detalles desde los BLs existentes
            var empleados  = new DetalleEmpleadoBL().GetAll(idProyecto);
            var materiales = new DetalleMaterialBL().GetAll(idProyecto);

            float totalEmp = empleados.Sum(e => e.Empleado.Sueldo + e.ValorGanancia);
            float totalMat = materiales.Sum(m => (m.Material.CostoPorUnidad + m.ValorGanancia) * m.Cantidad);
            float montoTotal = totalEmp + totalMat;

            var informe = new InformeMonto
            {
                IdProyecto      = idProyecto,
                TotalEmpleados  = totalEmp,
                TotalMateriales = totalMat,
                MontoTotal      = montoTotal
            };

            var uow = DalFactory.CreateUnitOfWork();
            var repo = DalFactory.CreateInformeMontoRepository(uow);

            uow.Begin();
            try
            {
                repo.Upsert(informe);
                uow.Commit();
                // Sin log: este método se llama cada vez que se refresca un proyecto, no es un evento de negocio.
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeMontoBL] Falla al recalcular informe de monto. Proy={idProyecto}", ex);
                throw;
            }

            return informe;
        }
    }
}
