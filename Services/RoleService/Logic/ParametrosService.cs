using DomainModel.Login;
using DomainModel.LoginDALInterfaces;
using Services.RoleService;
using System;

namespace Services.RoleService.Logic
{
    public class ParametrosService
    {
        private readonly IParametrosRepository _repo;

        public ParametrosService(IParametrosRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _repo.EnsureTableAndSeed();
        }

        public Parametros Obtener() => _repo.Obtener();

        public void Guardar(Parametros parametros)
        {
            if (parametros == null) throw new ArgumentNullException(nameof(parametros));
            parametros.UltimaModificacion = DateTime.Now;
            parametros.ModificadoPor = SessionContext.IdUsuario == Guid.Empty
                ? (Guid?)null
                : SessionContext.IdUsuario;

            _repo.Guardar(parametros);
            ParametrosContext.Cargar(parametros);
        }
    }
}
