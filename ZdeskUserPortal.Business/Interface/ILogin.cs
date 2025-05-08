using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business.Interface
{
    public interface ILogin
    {
        Task<LoginDTO> UserLogin(string username, string password);
    }
}
