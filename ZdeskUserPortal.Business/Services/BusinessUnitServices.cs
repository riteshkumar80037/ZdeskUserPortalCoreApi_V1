using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.Utility;

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
            try
            {
                return await _businessUnitRepo.GetAll(x => x.Active == true && x.BusinessUnitName != null);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
            
        }
    }
}
