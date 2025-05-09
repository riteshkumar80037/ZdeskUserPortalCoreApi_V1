using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model;

namespace ZdeskUserPortal.Business.Interface
{
    public interface IBusinessUnit
    {
        Task<IEnumerable<BusinessUnitEntity>> GetAllBusinessUnit();
    }
}
