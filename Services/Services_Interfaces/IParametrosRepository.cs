using DomainModel.Login;

namespace Services.Services_Interfaces
{
    public interface IParametrosRepository
    {
        Parametros Obtener();
        void Guardar(Parametros parametros);
        void EnsureTableAndSeed();
    }
}
