using DomainModel.Login;

namespace Services.RoleService
{
    /// <summary>
    /// Cache en memoria de los parámetros globales de la empresa.
    /// Se carga una vez al iniciar sesión y se actualiza al guardar cambios.
    /// </summary>
    public static class ParametrosContext
    {
        // Valores por defecto = los que estaban hardcodeados antes de esta feature
        public static decimal MargenEmpleados  { get; private set; } = 0.20m;
        public static decimal MargenMateriales { get; private set; } = 0.00m;
        public static decimal UtilidadEmpresa  { get; private set; } = 0.10m;

        public static void Cargar(Parametros p)
        {
            if (p == null) return;
            MargenEmpleados  = p.MargenEmpleados;
            MargenMateriales = p.MargenMateriales;
            UtilidadEmpresa  = p.UtilidadEmpresa;
        }
    }
}
