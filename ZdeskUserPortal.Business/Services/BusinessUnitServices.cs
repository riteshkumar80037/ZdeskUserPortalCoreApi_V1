using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;

namespace ZdeskUserPortal.Business.Services
{
    public class BusinessUnitServices : IBusinessUnit
    {
        private readonly IBaseRepository<BusinessUnitEntity> _businessUnitRepo;

        public BusinessUnitServices(IBaseRepository<BusinessUnitEntity> businessUnitRepository)
        {
            _businessUnitRepo = businessUnitRepository;
        }
        public async Task<IEnumerable<BusinessUnitEntity>> GetAllBusinessUnit()
        {
           return await _businessUnitRepo.GetAll(x=>x.Active==true && x.BusinessUnitName !=null);
        }
    }
}
