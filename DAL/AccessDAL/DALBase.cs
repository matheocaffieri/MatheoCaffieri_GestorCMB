using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.AccessDAL
{
    public abstract class DALBase
    {
        protected readonly string _cs;
        protected DALBase(string connectionString) => _cs = connectionString;
    }
}
