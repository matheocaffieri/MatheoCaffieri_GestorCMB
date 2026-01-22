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
        private readonly GestorCMBEntities _context;
        private readonly DbSet<ClienteEf> _set;

        private static readonly Expression<Func<ClienteEf, DomainModel.Cliente>> ToDomainExpr =
            c => new DomainModel.Cliente
            {
                IdCliente = c.idCliente,
                RazonSocial = c.razonSocial,
                Telefono = c.telefono,
                Mail = c.mail,
                NombreContacto = c.nombreContacto
            };

        public ClienteRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<ClienteEf>();
        }

        private static void MapToEf(DomainModel.Cliente src, ClienteEf dst)
        {
            dst.idCliente = src.IdCliente;
            dst.razonSocial = src.RazonSocial;
            dst.telefono = src.Telefono;
            dst.mail = src.Mail;
            dst.nombreContacto = src.NombreContacto;
        }

        public void Add(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new ClienteEf();
            MapToEf(entity, ef);

            _set.Add(ef);

            LoggerLogic.Info($"[ClienteRepository] Add en contexto (pendiente Commit). Id={entity.IdCliente}");
            // NO SaveChanges
        }

        public void Update(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdCliente);
            if (ef == null) throw new InvalidOperationException("Cliente no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;

            LoggerLogic.Info($"[ClienteRepository] Update en contexto (pendiente Commit). Id={entity.IdCliente}");
            // NO SaveChanges
        }

        public void Delete(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdCliente);
            if (ef == null) return;

            _set.Remove(ef);

            LoggerLogic.Info($"[ClienteRepository] Delete en contexto (pendiente Commit). Id={entity.IdCliente}");
            // NO SaveChanges
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
