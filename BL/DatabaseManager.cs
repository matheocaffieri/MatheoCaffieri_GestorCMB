using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DatabaseManager
    {
        private readonly DatabaseService _databaseService;

        public DatabaseManager()
        {
            _databaseService = new DatabaseService();
        }

        public void InitializeDatabase()
        {
            _databaseService.WarmUp(); 
        }
    }
}
