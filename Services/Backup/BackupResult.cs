namespace Services.Backup
{
    /// <summary>
    /// Resultado de intentar respaldar una base. Ok=false trae el motivo en Error.
    /// </summary>
    public class BackupResult
    {
        public string Base { get; set; }
        public bool Ok { get; set; }
        public string RutaArchivo { get; set; }
        public string Error { get; set; }
    }
}
