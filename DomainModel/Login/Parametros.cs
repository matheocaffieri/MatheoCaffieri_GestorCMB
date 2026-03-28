using System;

namespace DomainModel.Login
{
    public class Parametros
    {
        public Guid IdParametro { get; set; }
        public decimal MargenEmpleados { get; set; }
        public decimal MargenMateriales { get; set; }
        public decimal UtilidadEmpresa { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public Guid? ModificadoPor { get; set; }
    }
}
