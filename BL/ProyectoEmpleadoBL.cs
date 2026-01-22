using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Data.Entity.Core.EntityClient;

namespace BL
{
    public class ProyectoEmpleadoBL
    {
        private const int MAX_PROYECTOS_ACTIVOS = 3;

        public void AgregarEmpleadoDetalleProyecto(Guid idProyecto, Guid idEmpleado, double valorGanancia)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (idEmpleado == Guid.Empty) throw new ArgumentException("idEmpleado requerido.", nameof(idEmpleado));

            using (var ctx = new GestorCMBEntities())
            using (var uow = new SqlUnitOfWork(ctx))
            {
                uow.Begin();

                IEmpleadoRepository empRepo = new EmpleadoRepository(uow);
                IDetalleEmpleadosRepository detRepo = new DetalleEmpleadosRepository(uow);

                try
                {
                    if (detRepo.Exists(idProyecto, idEmpleado))
                        throw new InvalidOperationException("El empleado ya está agregado a este proyecto.");

                    var emp = empRepo.GetById(idEmpleado);
                    if (emp == null) throw new InvalidOperationException("Empleado no encontrado.");
                    if (!emp.IsActive) throw new InvalidOperationException("El empleado está inactivo.");

                    if (emp.CantidadProyectosActivos >= MAX_PROYECTOS_ACTIVOS)
                        throw new InvalidOperationException("El empleado ya tiene 3 proyectos activos. No se puede agregar.");

                    var det = new DetalleProyectoEmpleado
                    {
                        IdDetalleProyectoEmpleado = Guid.NewGuid(),
                        IdProyecto = idProyecto,
                        IdEmpleado = idEmpleado,
                        FechaIngresoEmpleado = DateTime.Now,
                        ValorGanancia = (float)valorGanancia
                    };

                    detRepo.Add(det, estado: "1"); // "1" activo

                    emp.CantidadProyectosActivos += 1;
                    empRepo.Update(emp);

                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoEmpleadoBL] Empleado agregado al proyecto. Proy={idProyecto} Emp={idEmpleado} Det={det.IdDetalleProyectoEmpleado}");
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoEmpleadoBL] Error al agregar empleado. Proy={idProyecto} Emp={idEmpleado}", ex);
                    throw;
                }
            }
        }

        public void QuitarEmpleadoDelProyecto(Guid idProyecto, Guid idEmpleado)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (idEmpleado == Guid.Empty) throw new ArgumentException("idEmpleado requerido.", nameof(idEmpleado));

            using (var ctx = new GestorCMBEntities())
            using (var uow = new SqlUnitOfWork(ctx))
            {
                uow.Begin();

                IEmpleadoRepository empRepo = new EmpleadoRepository(uow);
                IDetalleEmpleadosRepository detRepo = new DetalleEmpleadosRepository(uow);

                try
                {
                    var det = detRepo.GetByProyectoEmpleado(idProyecto, idEmpleado);
                    if (det == null) throw new InvalidOperationException("El empleado no está en este proyecto.");

                    detRepo.SetEstado(det.IdDetalleProyectoEmpleado, "0"); // "0" inactivo

                    var emp = empRepo.GetById(idEmpleado);
                    if (emp != null && emp.CantidadProyectosActivos > 0)
                    {
                        emp.CantidadProyectosActivos -= 1;
                        empRepo.Update(emp);
                    }

                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoEmpleadoBL] Empleado quitado del proyecto. Proy={idProyecto} Emp={idEmpleado} Det={det.IdDetalleProyectoEmpleado}");
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoEmpleadoBL] Error al quitar empleado. Proy={idProyecto} Emp={idEmpleado}", ex);
                    throw;
                }
            }
        }
    }
}
