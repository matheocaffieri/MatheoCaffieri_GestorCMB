using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

using DomainModel;
using DomainModel.Interfaces;

// Aliases EF (ajustá namespaces si difieren)
using DetEmpEf = DAL.Detalle_proyecto_empleado;
using EmpEf = DAL.Empleado;

namespace DAL.ProjectRepo
{
    public class DetalleEmpleadosRepository : IDetalleEmpleadosRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<DetEmpEf> _set;

        // Proyección EF -> Dominio (100% traducible a SQL)
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
            _uow = uow;

            var sqlUow = (DAL.FactoryDAL.SqlUnitOfWork)uow;
            var sqlConn = (SqlConnection)sqlUow.Connection;

            // Contexto temporal para obtener MetadataWorkspace
            using (var tmp = new GestorCMBEntities(
                       new EntityConnection("name=GestorCMBEntities"),
                       contextOwnsConnection: true))
            {
                var ws = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;
                var econn = new EntityConnection(ws, sqlConn);
                _context = new GestorCMBEntities(econn, contextOwnsConnection: false);
            }

            // Compartir transacción si existe
            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<DetEmpEf>();
        }

        public List<DomainModel.DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            // No hace falta Include cuando proyectás: EF genera el JOIN por los accesos a d.Empleado.*
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto)
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
