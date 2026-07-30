using DAL.DAL_Interfaces;
using DAL.Integridad;
using DAL.ProjectRepo;

namespace DAL.FactoryDAL
{
    // Fabrica de la unidad de trabajo y los repositorios del dominio (EF).
    // Centraliza el 'new' del contexto, el UnitOfWork y los repos para que la
    // capa de negocio (BL) no instancie EntityFramework directamente.
    public static class DalFactory
    {
        public static IUnitOfWork CreateUnitOfWork()
            => new SqlUnitOfWork(new GestorCMBEntities());

        public static IIntegridadRepository CreateIntegridadRepository() => new IntegridadRepository();

        public static ClienteRepository CreateClienteRepository(IUnitOfWork uow) => new ClienteRepository(uow);
        public static EmpleadoRepository CreateEmpleadoRepository(IUnitOfWork uow) => new EmpleadoRepository(uow);
        public static MaterialRepository CreateMaterialRepository(IUnitOfWork uow) => new MaterialRepository(uow);
        public static InventarioRepository CreateInventarioRepository(IUnitOfWork uow) => new InventarioRepository(uow);
        public static ProveedorRepository CreateProveedorRepository(IUnitOfWork uow) => new ProveedorRepository(uow);
        public static ProyectoRepository CreateProyectoRepository(IUnitOfWork uow) => new ProyectoRepository(uow);
        public static MaterialFaltanteRepository CreateMaterialFaltanteRepository(IUnitOfWork uow) => new MaterialFaltanteRepository(uow);
        public static InformeSnapshotFaltanteRepository CreateInformeSnapshotFaltanteRepository(IUnitOfWork uow) => new InformeSnapshotFaltanteRepository(uow);
        public static DetalleEmpleadosRepository CreateDetalleEmpleadosRepository(IUnitOfWork uow) => new DetalleEmpleadosRepository(uow);
        public static DetalleMaterialesRepository CreateDetalleMaterialesRepository(IUnitOfWork uow) => new DetalleMaterialesRepository(uow);
        public static InformeDeCompraRepository CreateInformeDeCompraRepository(IUnitOfWork uow) => new InformeDeCompraRepository(uow);
        public static InformeMontoRepository CreateInformeMontoRepository(IUnitOfWork uow) => new InformeMontoRepository(uow);
        public static DetalleInformeMaterialFaltanteRepository CreateDetalleInformeMaterialFaltanteRepository(IUnitOfWork uow) => new DetalleInformeMaterialFaltanteRepository(uow);
    }
}
