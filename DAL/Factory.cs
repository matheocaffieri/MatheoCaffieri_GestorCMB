using DAL.ProjectRepo;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Factory
    {
        #region Singleton
        private static readonly Factory _instance = new Factory();
        public static Factory Instance => _instance;
        private Factory() { }
        #endregion

        private readonly GestorCMBEntities _context = new GestorCMBEntities();

        public IGenericRepository<Proveedor> GetProveedorRepository()
        {
            return new ProjectRepo.ProveedorRepository(_context) as IGenericRepository<Proveedor>;
        }

        public IGenericRepository<Empleado> GetEmpleadoRepository()
        {
            return new ProjectRepo.EmpleadoRepository(_context) as IGenericRepository<Empleado>;
        }

        public IGenericRepository<Proyecto> GetProyectoRepository()
        {
            return new ProjectRepo.ProyectoRepository(_context) as IGenericRepository<Proyecto>;
        }
    }
}
