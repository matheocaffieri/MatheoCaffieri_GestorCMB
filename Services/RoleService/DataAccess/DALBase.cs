using System.Data.SqlClient;

namespace Services.RoleService.DataAccess
{
    public abstract class DALBase
    {
        protected readonly string _cs;
        protected DALBase(string connectionString) => _cs = connectionString;
    }
}
