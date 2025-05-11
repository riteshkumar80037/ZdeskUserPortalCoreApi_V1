using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZdeskUserPortal.DataAccess.Home
{
    public class LoginAccess : ZdeskDataAccess
    {
        public static LoginAccess Instance { get; } = new LoginAccess();
        private const string USER_LOGIN_SELECT = "User_Login_Select";
        private LoginAccess() : base()
        {
        }
        public LoginAccess(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public async Task<UsersEntity> Login(string userEmail,string password)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", userEmail, DbType.String, ParameterDirection.Input);
                parameters.Add("@Password", password, DbType.String, ParameterDirection.Input);

                return GetValueByProcedure<UsersEntity, DynamicParameters>(USER_LOGIN_SELECT, parameters, CONNECTION_STRING_NAME);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error in Data Access while getting {GetType().FullName}.", ex);
            }
           

        }
    }
}
