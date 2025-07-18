using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProjectRepo
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly GestorCMBEntities _context;

        // Constructor para inyectar el contexto
        public ProveedorRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(DomainModel.Proveedor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                var proveedor = new Proveedor
                {
                    idProveedor = entity.IdProveedor,
                    descripcion = entity.Descripcion,
                    telefono = entity.Telefono,
                    isActive = entity.IsActive
                };

                _context.Proveedor.Add(proveedor);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el proveedor.", ex);
            }
        }

        public void Delete(DomainModel.Proveedor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                var proveedor = _context.Proveedor.FirstOrDefault(p => p.idProveedor == entity.IdProveedor);

                if (proveedor == null)
                    throw new KeyNotFoundException("Proveedor no encontrado.");

                _context.Proveedor.Remove(proveedor);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el proveedor.", ex);
            }
        }

        public DomainModel.Proveedor GetById(Guid id)
        {
            try
            {
                var proveedor = _context.Proveedor.FirstOrDefault(p => p.idProveedor == id);

                if (proveedor == null)
                    throw new KeyNotFoundException("Proveedor no encontrado.");

                return new DomainModel.Proveedor
                {
                    IdProveedor = proveedor.idProveedor,
                    Descripcion = proveedor.descripcion,
                    Telefono = proveedor.telefono,
                    IsActive = proveedor.isActive
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el proveedor.", ex);
            }
        }

        public void Update(DomainModel.Proveedor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                var proveedor = _context.Proveedor.FirstOrDefault(p => p.idProveedor == entity.IdProveedor);

                if (proveedor == null)
                    throw new KeyNotFoundException("Proveedor no encontrado.");

                proveedor.descripcion = entity.Descripcion;
                proveedor.telefono = entity.Telefono;
                proveedor.isActive = entity.IsActive;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el proveedor.", ex);
            }
        }

        public List<DomainModel.Proveedor> GetAll()
        {
            return _context.Proveedor
                .AsNoTracking() // Mejora el rendimiento en consultas de solo lectura
                .Select(p => new DomainModel.Proveedor
                {
                    IdProveedor = p.idProveedor,
                    Descripcion = p.descripcion,
                    Telefono = p.telefono,
                    IsActive = p.isActive
                })
                .ToList();
        }

    }
}

