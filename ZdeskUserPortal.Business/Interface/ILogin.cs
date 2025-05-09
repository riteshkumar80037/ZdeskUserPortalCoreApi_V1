using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business.Interface
{
    public interface ILogin
    {
        Task<BigInteger> UserLogin(string username, string password);
    }
}
