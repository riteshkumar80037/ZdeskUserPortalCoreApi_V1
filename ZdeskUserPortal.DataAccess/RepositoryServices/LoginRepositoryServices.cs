using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.DataAccess.Home;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.RepositoryInterfaces;

namespace ZdeskUserPortal.DataAccess.RepositoryServices
{
    public class LoginRepositoryServices : ILoginRepository
    {
        public async Task<UsersEntity> Login(string email, string password)
        {
           return await LoginAccess.Instance.Login(email, password);
        }
    }
}
