using DomainModel.Login;

namespace DomainModel.LoginDALInterfaces
{
    public interface IParametrosRepository
    {
        Parametros Obtener();
        void Guardar(Parametros parametros);
        void EnsureTableAndSeed();
    }
}
