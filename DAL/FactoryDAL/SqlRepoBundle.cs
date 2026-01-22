using DomainModel.LoginDALInterfaces;
using DomainModel.Interfaces;
using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DAL.LoginDAL;

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

        public SqlRepoBundle(IUnitOfWork uowEf, ILoginUnitOfWork uowLogin)
        {
            Usuarios = new DAL.LoginDAL.UsuarioRepository(uowLogin);

            Empleados = new EmpleadoRepository(uowEf);
            Inventario = new InventarioRepository(uowEf);
            Materiales = new MaterialRepository(uowEf);
            MaterialesFaltantes = new MaterialFaltanteRepository(uowEf);
            Proveedores = new ProveedorRepository(uowEf);
            Proyectos = new ProyectoRepository(uowEf);
            DetalleEmpleados = new DetalleEmpleadosRepository(uowEf);
            DetalleMateriales = new DetalleMaterialesRepository(uowEf);
        }

    }
}
