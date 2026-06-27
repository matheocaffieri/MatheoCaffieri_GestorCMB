using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DAL.DAL_Interfaces;
using Services.Logs;
using System;
using System.Data.Entity.Core.EntityClient;

using BL.BL_Interfaces;

namespace BL
{
    public class ProyectoEmpleadoBL : IProyectoEmpleadoBL
    {
        private const int MAX_PROYECTOS_ACTIVOS = 3;

        public void AgregarEmpleadoDetalleProyecto(Guid idProyecto, Guid idEmpleado, double valorGanancia)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");

            using (var ctx = new GestorCMBEntities())
            using (var uow = new SqlUnitOfWork(ctx))
            {
                uow.Begin();

                IEmpleadoRepository empRepo = new EmpleadoRepository(uow);
                IDetalleEmpleadosRepository detRepo = new DetalleEmpleadosRepository(uow);

                try
                {
                    if (detRepo.Exists(idProyecto, idEmpleado))
                        throw new AppException("err_empleado_ya_en_proyecto");

                    var emp = empRepo.GetById(idEmpleado);
                    if (emp == null) throw new AppException("err_empleado_not_found");
                    if (!emp.IsActive) throw new AppException("err_empleado_inactivo");

                    if (emp.CantidadProyectosActivos >= MAX_PROYECTOS_ACTIVOS)
                        throw new AppException("err_empleado_max_proyectos");

                    var det = new DetalleProyectoEmpleado
                    {
                        IdDetalleProyectoEmpleado = Guid.NewGuid(),
                        IdProyecto = idProyecto,
                        IdEmpleado = idEmpleado,
                        FechaIngresoEmpleado = DateTime.Now,
                        ValorGanancia = (float)valorGanancia
                    };

                    detRepo.Add(det, estado: "1");

                    emp.CantidadProyectosActivos += 1;
                    empRepo.Update(emp);

                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoEmpleadoBL] Empleado agregado al proyecto. Proy={idProyecto} Emp={idEmpleado} Det={det.IdDetalleProyectoEmpleado}");
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoEmpleadoBL] Validación al agregar empleado al proyecto: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoEmpleadoBL] Falla al agregar empleado. Proy={idProyecto} Emp={idEmpleado}", ex);
                    throw;
                }
            }
        }

        public void QuitarEmpleadoDelProyecto(Guid idProyecto, Guid idEmpleado)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");

            using (var ctx = new GestorCMBEntities())
            using (var uow = new SqlUnitOfWork(ctx))
            {
                uow.Begin();

                IEmpleadoRepository empRepo = new EmpleadoRepository(uow);
                IDetalleEmpleadosRepository detRepo = new DetalleEmpleadosRepository(uow);

                try
                {
                    var det = detRepo.GetByProyectoEmpleado(idProyecto, idEmpleado);
                    if (det == null) throw new AppException("err_empleado_no_en_proyecto");

                    detRepo.SetEstado(det.IdDetalleProyectoEmpleado, "0");

                    var emp = empRepo.GetById(idEmpleado);
                    if (emp != null && emp.CantidadProyectosActivos > 0)
                    {
                        emp.CantidadProyectosActivos -= 1;
                        empRepo.Update(emp);
                    }

                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoEmpleadoBL] Empleado quitado del proyecto. Proy={idProyecto} Emp={idEmpleado} Det={det.IdDetalleProyectoEmpleado}");
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoEmpleadoBL] Validación al quitar empleado: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoEmpleadoBL] Falla al quitar empleado. Proy={idProyecto} Emp={idEmpleado}", ex);
                    throw;
                }
            }
        }
    }
}
