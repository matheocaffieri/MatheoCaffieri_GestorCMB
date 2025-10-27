using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using EmpleadoDom = DomainModel.Empleado;
// Cambiá este namespace por el REAL donde vive tu entidad EF “Empleado”
using EmpleadoEf = DAL.Empleado;


namespace DAL.ProjectRepo
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<EmpleadoEf> _set;

        // ===== Proyección EF -> Dominio (traducible a SQL) =====
        private static readonly Expression<Func<EmpleadoEf, EmpleadoDom>> ToDomainExpr =
            e => new EmpleadoDom
            {
                IdEmpleado = e.idEmpleado,
                Nombre = e.nombre,
                Apellido = e.apellido,
                NroDocumento = e.nroDocumento,
                Sueldo = (float)e.sueldo,           // EF: decimal → Dominio: float
                CantidadProyectosActivos = e.cantidadProyectosActivos,
                IsActive = e.isActive
            };

        public EmpleadoRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _uow = uow;

            var sqlUow = (DAL.FactoryDAL.SqlUnitOfWork)uow;
            var sqlConn = (SqlConnection)sqlUow.Connection;

            // 1) Crear un contexto "temporal" solo para obtener el MetadataWorkspace del EDMX.
            //    Usamos EntityConnection("name=GestorCMBEntities") porque tu contexto no tiene ctor vacío.
            using (var tmp = new GestorCMBEntities(
                       new EntityConnection("name=GestorCMBEntities"),
                       contextOwnsConnection: true))
            {
                var workspace = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;

                // 2) Armar una EntityConnection que:
                //    - Usa los metadatos del EDMX (workspace)
                //    - Reutiliza la MISMA SqlConnection del UoW (compartimos conexión/transacción)
                var entityConn = new EntityConnection(workspace, sqlConn);

                // 3) Instanciar el contexto real (Database-First) sin ser dueño de la conexión
                _context = new GestorCMBEntities(entityConn, contextOwnsConnection: false);
            }

            // 4) Reutilizar la transacción del UoW si existe
            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<EmpleadoEf>();
        }

        // ===== Map Dominio -> EF (para Add/Update) =====
        private static void MapToEf(EmpleadoDom src, EmpleadoEf dst)
        {
            dst.idEmpleado = src.IdEmpleado;
            dst.nombre = src.Nombre;
            dst.apellido = src.Apellido;
            dst.nroDocumento = src.NroDocumento;
            dst.sueldo = (float)src.Sueldo;  // Dominio: float → EF: decimal
            dst.cantidadProyectosActivos = src.CantidadProyectosActivos;
            dst.isActive = src.IsActive;
        }

        // ===== CRUD =====

        public void Add(EmpleadoDom entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = new EmpleadoEf();
            MapToEf(entity, ef);
            _set.Add(ef);
            _context.SaveChanges();
        }

        public void Update(EmpleadoDom entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _set.Find(entity.IdEmpleado);
            if (ef == null) throw new InvalidOperationException("Empleado no encontrado.");
            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(EmpleadoDom entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _set.Find(entity.IdEmpleado);
            if (ef == null) return;
            _set.Remove(ef);
            _context.SaveChanges();
        }

        public EmpleadoDom GetById(Guid id)
        {
            // Proyecta con Expression → SQL válido; no llama métodos .NET en el Select
            return _set.AsNoTracking()
                       .Where(x => x.idEmpleado == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<EmpleadoDom> GetAll()
        {
            // Proyección traducible a SQL
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
