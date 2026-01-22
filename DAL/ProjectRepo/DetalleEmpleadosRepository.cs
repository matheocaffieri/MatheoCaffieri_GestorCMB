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

// EF aliases
using DetEmpEf = DAL.Detalle_proyecto_empleado;

namespace DAL.ProjectRepo
{
    public class DetalleEmpleadosRepository : IDetalleEmpleadosRepository
    {
        private readonly GestorCMBEntities _context;
        private readonly DbSet<DetEmpEf> _set;

        private static readonly Expression<Func<DetEmpEf, DomainModel.DetalleProyectoEmpleado>> ToDomainExpr =
            d => new DomainModel.DetalleProyectoEmpleado
            {
                IdDetalleProyectoEmpleado = d.idDetalleEmpleado,
                IdProyecto = d.idProyecto,
                IdEmpleado = d.idEmpleado,
                FechaIngresoEmpleado = d.fechaIngresoEmpleado,
                ValorGanancia = (float)d.valorGanancia,

                Empleado = new DomainModel.Empleado
                {
                    IdEmpleado = d.Empleado.idEmpleado,
                    Nombre = d.Empleado.nombre,
                    Apellido = d.Empleado.apellido,
                    NroDocumento = d.Empleado.nroDocumento,
                    Sueldo = (float)d.Empleado.sueldo,
                    CantidadProyectosActivos = d.Empleado.cantidadProyectosActivos,
                    IsActive = d.Empleado.isActive
                }
            };

        public DetalleEmpleadosRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<DetEmpEf>();
        }

        public List<DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public bool Exists(Guid idProyecto, Guid idEmpleado)
        {
            return _set.AsNoTracking()
                       .Any(d => d.idProyecto == idProyecto && d.idEmpleado == idEmpleado && d.estado == "1");
        }

        public void Add(DetalleProyectoEmpleado detalle, string estado = "1")
        {
            if (detalle == null) throw new ArgumentNullException(nameof(detalle));

            var newId = detalle.IdDetalleProyectoEmpleado == Guid.Empty ? Guid.NewGuid() : detalle.IdDetalleProyectoEmpleado;

            var nuevo = new DetEmpEf
            {
                idDetalleEmpleado = newId,
                idProyecto = detalle.IdProyecto,
                idEmpleado = detalle.IdEmpleado,
                fechaIngresoEmpleado = detalle.FechaIngresoEmpleado == default(DateTime) ? DateTime.Now : detalle.FechaIngresoEmpleado,
                valorGanancia = detalle.ValorGanancia,
                estado = string.IsNullOrWhiteSpace(estado) ? "1" : estado
            };

            _set.Add(nuevo);
            detalle.IdDetalleProyectoEmpleado = newId; // ya lo tenés
            // NO SaveChanges
        }

        public DetalleProyectoEmpleado GetByProyectoEmpleado(Guid idProyecto, Guid idEmpleado)
        {
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto && d.idEmpleado == idEmpleado)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public void Update(DetalleProyectoEmpleado detalle)
        {
            if (detalle == null) throw new ArgumentNullException(nameof(detalle));
            if (detalle.IdDetalleProyectoEmpleado == Guid.Empty)
                throw new ArgumentException("IdDetalleProyectoEmpleado requerido.", nameof(detalle));

            var row = _set.SingleOrDefault(d => d.idDetalleEmpleado == detalle.IdDetalleProyectoEmpleado);
            if (row == null) throw new InvalidOperationException("No existe el detalle de empleado.");

            row.fechaIngresoEmpleado = detalle.FechaIngresoEmpleado;
            row.valorGanancia = detalle.ValorGanancia;

            // NO SaveChanges
        }

        public void SetEstado(Guid idDetalleEmpleado, string estado)
        {
            if (idDetalleEmpleado == Guid.Empty)
                throw new ArgumentException("idDetalleEmpleado requerido.", nameof(idDetalleEmpleado));

            var row = _set.SingleOrDefault(d => d.idDetalleEmpleado == idDetalleEmpleado);
            if (row == null) throw new InvalidOperationException("No existe el detalle de empleado.");

            row.estado = string.IsNullOrWhiteSpace(estado) ? "0" : estado;
            // NO SaveChanges
        }
    }
}
