using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.ProjectRepo
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly GestorCMBEntities _context;

        public EmpleadoRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(DomainModel.Empleado entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DomainModel.Empleado entity)
        {
            throw new NotImplementedException();
        }

        public List<DomainModel.Empleado> GetAll()
        {
            return _context.Empleado
                .AsNoTracking() 
                .Select(e => new DomainModel.Empleado
                {
                    IdEmpleado = e.idEmpleado,
                    Nombre = e.nombre,
                    Apellido = e.apellido,
                    NroDocumento = e.nroDocumento,
                    Sueldo = (float)e.sueldo, 
                    CantidadProyectosActivos = e.cantidadProyectosActivos,
                    IsActive = e.isActive
                })
                .ToList();
        }


        public DomainModel.Empleado GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(DomainModel.Empleado entity)
        {
            throw new NotImplementedException();
        }
    }
}
