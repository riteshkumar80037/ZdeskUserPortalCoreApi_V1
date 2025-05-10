using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;

namespace ZdeskUserPortal.Business.Services
{
    public class MasterServices : IMaster
    {
        private readonly IBaseRepository<BusinessUnitEntity> _businessUnitRepo;
        private readonly IDistributedCache _cache;

        public MasterServices(IBaseRepository<BusinessUnitEntity> businessUnitRepository, IDistributedCache cache)
        {
            _businessUnitRepo = businessUnitRepository;
            _cache = cache;
        }
        public async Task<IEnumerable<BusinessUnitEntity>> GetAllBusinessUnit()
        {
            IEnumerable<BusinessUnitEntity> businessUnitEntities=null;
            string businessUnits = await _cache.GetStringAsync(RedisKey.BUSINESSUNIT);

            if (string.IsNullOrEmpty(businessUnits))
            {
                businessUnitEntities= await _businessUnitRepo.GetAll(x => x.Active == true && x.BusinessUnitName != null);
                var jsonBusinessUnits = JsonSerializer.Serialize(businessUnitEntities);
                await _cache.SetStringAsync(RedisKey.BUSINESSUNIT, jsonBusinessUnits, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(60)
                });
            }
            else
            {
                businessUnitEntities = JsonSerializer.Deserialize<IEnumerable<BusinessUnitEntity>>(businessUnits);
            }
            return businessUnitEntities;
        }
    }
}
