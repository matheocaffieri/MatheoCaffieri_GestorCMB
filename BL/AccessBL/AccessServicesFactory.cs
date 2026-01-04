using DAL.AccessDAL;
using DomainModel.LoginDALInterfaces;
using Services.RoleService.Logic;

namespace BL.AccessBL
{
    public static class AccessServicesFactory
    {
        public static AccesoService CreateAccesoService(string cs)
        {
            IAccesoRepository accRepo = new AccesoRepository(cs);
            return new AccesoService(accRepo);
        }

        public static RolesService CreateRolesService(string cs)
        {
            IFamiliaRepository famRepo = new FamiliaRepository(cs);
            IAccesoRepository accRepo = new AccesoRepository(cs);
            return new RolesService(famRepo, accRepo);
        }

        public static UsuarioPermisosService CreateUsuarioPermisosService(string cs)
        {
            IUsuarioAccesoRepository uaRepo = new UsuarioAccesoRepository(cs);
            IAccesoRepository accRepo = new AccesoRepository(cs);
            return new UsuarioPermisosService(uaRepo, accRepo);
        }
    }
}
