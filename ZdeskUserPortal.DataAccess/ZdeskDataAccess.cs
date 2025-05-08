using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.DataAccess
{
    public abstract class ZdeskDataAccess : SqlDataAccess
    {
        protected const string CONNECTION_STRING_NAME = "ZdeskConnection";

        protected ZdeskDataAccess()
            : base()
        {
        }

        public ZdeskDataAccess(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }
    }
}
