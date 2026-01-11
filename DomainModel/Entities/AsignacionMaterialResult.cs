using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class AsignacionMaterialResult
    {
        public int StockInicial { get; }
        public int Solicitado { get; }
        public int Asignado { get; }
        public int Faltante { get; }

        public AsignacionMaterialResult(int stockInicial, int solicitado, int asignado, int faltante)
        {
            StockInicial = stockInicial;
            Solicitado = solicitado;
            Asignado = asignado;
            Faltante = faltante;
        }
    }
}
