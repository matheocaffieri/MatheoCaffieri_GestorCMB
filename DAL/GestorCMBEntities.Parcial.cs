using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class GestorCMBEntities : DbContext
    {
        public GestorCMBEntities(EntityConnection entityConnection, bool contextOwnsConnection)
            : base(entityConnection, contextOwnsConnection)
        {
        }
    }
}
