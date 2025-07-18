using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DatabaseService
    {
        public void WarmUp()
        {
            using (var context = new GestorCMBEntities())
            {
                // Realizar alguna operación para calentar el contexto
                var count = context.Proveedor.Count();
            }
        }
    }
}
