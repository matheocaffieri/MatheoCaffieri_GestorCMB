using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public enum EnumEstado
    {
        [Description("En proceso")]
        EnProceso,

        [Description("Suspendido")]
        Suspendido,

        [Description("Finalizado")]
        Finalizado
    }


}
