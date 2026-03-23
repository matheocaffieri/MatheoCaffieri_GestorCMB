using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using Services.Logs;
using System;
using System.Linq;

namespace BL
{
    public class InformeMontoBL
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

            LoggerLogic.Info($"[InformeMontoBL] Recalcular START. idProyecto={idProyecto} | emp={totalEmp} mat={totalMat} total={montoTotal}");

            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);
            var repo = new InformeMontoRepository(uow);

            uow.Begin();
            try
            {
                repo.Upsert(informe);
                uow.Commit();
                LoggerLogic.Info($"[InformeMontoBL] Recalcular OK. idProyecto={idProyecto}");
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeMontoBL] Recalcular ERROR. idProyecto={idProyecto}. {ex.Message}");
                throw;
            }

            return informe;
        }
    }
}
