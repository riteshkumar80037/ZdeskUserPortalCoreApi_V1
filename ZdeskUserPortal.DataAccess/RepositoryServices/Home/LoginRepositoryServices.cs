using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ZdeskUserPortal.DataAccess.Home;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.RepositoryInterfaces.Login;
using ZdeskUserPortal.Utility;

namespace ZdeskUserPortal.DataAccess.RepositoryServices.Home
{
    public class LoginRepositoryServices : ILoginRepository
    {
        public async Task<UsersEntity> Login(string email, string password)
        {
            try
            {
                return await LoginAccess.Instance.Login(email, password);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error in Data Access while getting {GetType().FullName}.", ex);
            }

        }
    }
}
