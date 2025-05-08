using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model;

namespace ZdeskUserPortal.Domain.RepositoryInterfaces
{
    public interface ILoginRepository
    {
        Task<UsersEntity> Login(string email, string password);
    }
}
