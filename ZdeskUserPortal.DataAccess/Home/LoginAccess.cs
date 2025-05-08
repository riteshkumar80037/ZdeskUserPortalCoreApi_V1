using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
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

        public DateTime  test()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CountryCode", 1, DbType.String, ParameterDirection.Input);
            parameters.Add("@InDate", System.DateTime.Now, DbType.Date, ParameterDirection.Input);

            return GetValueByProcedure<DateTime, DynamicParameters>(PROC_LASTBUSINESSDAY, parameters, CONNECTION_STRING_NAME);

        }
    }
}
