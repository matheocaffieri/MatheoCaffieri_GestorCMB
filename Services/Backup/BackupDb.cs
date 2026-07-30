namespace Services.Backup
{
    /// <summary>
    /// Representa una base de datos respaldable, derivada de una connection string del App.config.
    /// </summary>
    public class BackupDb
    {
        /// <summary>Nombre del catálogo SQL Server (Initial Catalog).</summary>
        public string Catalogo { get; set; }

        /// <summary>Nombre de la connection string en el App.config de donde salió.</summary>
        public string ConnectionStringName { get; set; }
    }
}
