using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.LoginInterfaces
{
    public interface IAuthAppService
    {
      //  Usuario Login(string mail, string password);
        void Logout();
        bool IsAuthenticated();
    }
}
