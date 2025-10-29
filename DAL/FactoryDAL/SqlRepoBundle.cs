using DAL.LoginDAL;
using DomainModel.Interfaces;
using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.FactoryDAL;
using DAL.ProjectRepo;

namespace DAL.FactoryDAL
{
    public class SqlRepoBundle : IRepoBundle
    {
        public IUsuarioRepository Usuarios { get; private set; } //DONE
        public IEmpleadoRepository Empleados { get; private set; } //DONE
        public IInformeMontoRepository InformesMonto { get; private set; }
        public IInventarioRepository Inventario { get; private set; } //DONE
        public IMaterialesFaltantesRepository MaterialesFaltantes { get; private set; } //DONE
        public IMaterialRepository Materiales { get; private set; } //DONE
        public IProveedorRepository Proveedores { get; private set; } //DONE
        public IProyectoRepository Proyectos { get; private set; } //DONE
        public IDetalleEmpleadosRepository DetalleEmpleados { get; private set; }
        public IDetalleMaterialesRepository DetalleMateriales { get; private set; }

        public SqlRepoBundle(IUnitOfWork uow)
        {
            Usuarios = new UsuarioRepository(uow); // OK
            Empleados = new EmpleadoRepository(uow); // Pendiente de implementar
            Inventario = new InventarioRepository(uow);
            Materiales = new MaterialRepository(uow);
            MaterialesFaltantes = new MaterialFaltanteRepository(uow);
            Proveedores = new ProveedorRepository(uow);
            Proyectos = new ProyectoRepository(uow);
            DetalleEmpleados = new DetalleEmpleadosRepository(uow);
            DetalleMateriales = new DetalleMaterialesRepository(uow);
        }
    }
}
