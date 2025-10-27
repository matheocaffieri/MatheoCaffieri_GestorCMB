using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public sealed class Factory
    {
        // ... CreateScope() como lo tenés

        public sealed class RepoScope : IDisposable
        {
            private readonly IUnitOfWork _uow;

            public IGenericRepository<DomainModel.Empleado> Empleados { get; }
            public IGenericRepository<DomainModel.Proveedor> Proveedores { get; }
            public IGenericRepository<DomainModel.Proyecto> Proyectos { get; }
            public IGenericRepository<DomainModel.Material> Materiales { get; }
            public IGenericRepository<DomainModel.Inventario> Inventario { get; }
            public IGenericRepository<DomainModel.MaterialFaltante> MaterialesFaltantes { get; }
            public IGenericRepository<DomainModel.InformeDeCompra> InformesDeCompra { get; }
            public IGenericRepository<DomainModel.Cliente> Clientes { get; }
            public IGenericRepository<DomainModel.DetalleProyectoEmpleado> DetallesProyectoEmpleado { get; }
            public IGenericRepository<DomainModel.DetalleProyectoMaterial> DetallesProyectoMaterial { get; }

            internal RepoScope(IUnitOfWork uow)
            {
                _uow = uow;

                // No castees, ya implementan la interfaz correcta
                Empleados = new EmpleadoRepository(uow);
                
            }

            public void Commit() => _uow.Commit();
            public void Rollback() => _uow.Rollback();
            public void Dispose() => _uow.Dispose();
        }
    }
}
