using DAL.FactoryDAL;
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

// Alias a la entidad EF (ajustá el namespace si difiere)
using ClienteEf = DAL.Cliente;

namespace DAL.ProjectRepo
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<ClienteEf> _set;

        // Proyección EF -> Dominio (100% traducible a SQL)
        private static readonly Expression<Func<ClienteEf, DomainModel.Cliente>> ToDomainExpr =
            c => new DomainModel.Cliente
            {
                IdCliente = c.idCliente,
                RazonSocial = c.razonSocial,
                Telefono = c.telefono,
                Mail = c.mail,
                NombreContacto = c.nombreContacto,
                // agregá acá cualquier otro campo de dominio que tengas
            };

        public ClienteRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _uow = uow;

            var sqlUow = (SqlUnitOfWork)uow;
            var sqlConn = (SqlConnection)sqlUow.Connection;

            // Contexto temporal sólo para obtener MetadataWorkspace
            using (var tmp = new GestorCMBEntities(
                       new EntityConnection("name=GestorCMBEntities"),
                       contextOwnsConnection: true))
            {
                var workspace = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;

                // EntityConnection que reutiliza la MISMA SqlConnection del UoW
                var entityConn = new EntityConnection(workspace, sqlConn);

                // Contexto real (no dueño de la conexión)
                _context = new GestorCMBEntities(entityConn, contextOwnsConnection: false);
            }

            // Compartimos transacción del UoW si existe
            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<ClienteEf>();
        }

        // ===== Map Dominio -> EF (para altas/ediciones) =====
        private static void MapToEf(DomainModel.Cliente src, ClienteEf dst)
        {
            dst.idCliente = src.IdCliente;
            dst.razonSocial = src.RazonSocial;
            dst.telefono = src.Telefono;
            dst.mail = src.Mail;
            dst.nombreContacto = src.NombreContacto;
            // mapeá acá cualquier otra columna que tengas en la tabla
        }

        // ===== CRUD =====

        public void Add(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new ClienteEf();
            MapToEf(entity, ef);

            _set.Add(ef);

            var rows = _context.SaveChanges();
            LoggerLogic.Info($"[ClienteRepository] SaveChanges filas afectadas (Add): {rows}");
        }

        public void Update(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdCliente);
            if (ef == null) throw new InvalidOperationException("Cliente no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;

            var rows = _context.SaveChanges();
            LoggerLogic.Info($"[ClienteRepository] SaveChanges filas afectadas (Update): {rows}");
        }

        public void Delete(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdCliente);
            if (ef == null) return;

            _set.Remove(ef);

            var rows = _context.SaveChanges();
            LoggerLogic.Info($"[ClienteRepository] SaveChanges filas afectadas (Delete): {rows}");
        }

        public DomainModel.Cliente GetById(Guid id)
        {
            return _set.AsNoTracking()
                       .Where(c => c.idCliente == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<DomainModel.Cliente> GetAll()
        {
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
