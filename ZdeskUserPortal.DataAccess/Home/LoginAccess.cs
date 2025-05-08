using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ZdeskUserPortal.Domain.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZdeskUserPortal.DataAccess.Home
{
    public class LoginAccess : ZdeskDataAccess
    {
        public static LoginAccess Instance { get; } = new LoginAccess();
        private const string PROC_LASTBUSINESSDAY = "dbo.General_IsHoliday";
        private LoginAccess() : base()
        {
        }
        public LoginAccess(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public async Task<UsersEntity> Login(string userEmail,string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CountryCode", userEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@InDate", password, DbType.String, ParameterDirection.Input);

            return  GetValueByProcedure<UsersEntity, DynamicParameters>(PROC_LASTBUSINESSDAY, parameters, CONNECTION_STRING_NAME);

        }
    }
}
