using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using DAL.FactoryDAL;

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
        private readonly GestorCMBEntities _context;
        private readonly DbSet<EmpleadoEf> _set;

        private static readonly Expression<Func<EmpleadoEf, EmpleadoDom>> ToDomainExpr =
            e => new EmpleadoDom
            {
                IdEmpleado = e.idEmpleado,
                Nombre = e.nombre,
                Apellido = e.apellido,
                NroDocumento = e.nroDocumento,
                Sueldo = (float)e.sueldo,
                CantidadProyectosActivos = e.cantidadProyectosActivos,
                IsActive = e.isActive
            };

        public EmpleadoRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<EmpleadoEf>();
        }

        private static void MapToEf(EmpleadoDom src, EmpleadoEf dst)
        {
            dst.idEmpleado = src.IdEmpleado;
            dst.nombre = src.Nombre;
            dst.apellido = src.Apellido;
            dst.nroDocumento = src.NroDocumento;

            // si EF sueldo es decimal, lo correcto sería: dst.sueldo = (decimal)src.Sueldo;
            // dejo tu lógica "mínimo cambio", pero esto conviene corregir.
            dst.sueldo = (float)src.Sueldo;

            dst.cantidadProyectosActivos = src.CantidadProyectosActivos;
            dst.isActive = src.IsActive;
        }

        public void Add(EmpleadoDom entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = new EmpleadoEf();
            MapToEf(entity, ef);
            _set.Add(ef);
            // NO SaveChanges
        }

        public void Update(EmpleadoDom entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _set.Find(entity.IdEmpleado);
            if (ef == null) throw new InvalidOperationException("Empleado no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            // NO SaveChanges
        }

        public void Delete(EmpleadoDom entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _set.Find(entity.IdEmpleado);
            if (ef == null) return;

            _set.Remove(ef);
            // NO SaveChanges
        }

        public EmpleadoDom GetById(Guid id)
        {
            return _set.AsNoTracking()
                       .Where(x => x.idEmpleado == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<EmpleadoDom> GetAll()
        {
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
