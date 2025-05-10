using AutoMapper;
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
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.Domain.RepositoryInterfaces.Login;
using ZdeskUserPortal.DTOModel;
using ZdeskUserPortal.Utility;

namespace ZdeskUserPortal.Business.Services
{
    public class MasterServices : IMaster
    {
        private readonly IBaseRepository<BusinessUnitEntity> _businessUnitRepo;
        private readonly IBaseRepository<OrganizationEntity> _organazationRepo;
        private readonly IBaseRepository<SmtpEntity> _smtpRepo;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public MasterServices(IBaseRepository<BusinessUnitEntity> businessUnitRepository,
            IBaseRepository<OrganizationEntity> organizationRepository, IDistributedCache cache,
            IMapper mapper, IBaseRepository<SmtpEntity> smtpRepo)
        {
            _businessUnitRepo = businessUnitRepository;
            _organazationRepo = organizationRepository;
            _cache = cache;
            _mapper = mapper;
            _smtpRepo = smtpRepo;
        }
        public async Task<IEnumerable<BusinessUnitEntity>> GetAllBusinessUnit()
        {
            try
            {
                IEnumerable<BusinessUnitEntity> businessUnitEntities = null;
                string businessUnits = await _cache.GetStringAsync(RedisKey.BUSINESSUNIT);

                if (string.IsNullOrEmpty(businessUnits))
                {
                    businessUnitEntities = await _businessUnitRepo.GetAll(x => x.Active == true && x.BusinessUnitName != null);
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
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }

           
        }

        public async Task<LogoDTO> OrganizationDetail()
        {
            try
            {
                OrganizationEntity organizationEntity = null;
                var organizationCache = await _cache.GetStringAsync(RedisKey.ORGANIZATION);

                if (string.IsNullOrEmpty(organizationCache))
                {
                    var resutl = await _organazationRepo.GetAll();
                    var jsonBusinessUnits = JsonSerializer.Serialize(resutl.FirstOrDefault());
                    await _cache.SetStringAsync(RedisKey.ORGANIZATION, jsonBusinessUnits, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(60)
                    });
                }
                else
                {
                    organizationEntity = JsonSerializer.Deserialize<OrganizationEntity>(organizationCache);
                }
                return _mapper.Map<LogoDTO>(organizationEntity);
                
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
        }

        public async Task<SmtpEntity> SmtpDetail()
        {
            SmtpEntity smtpEntity = new SmtpEntity();
            try
            {
                
                var smtpCache = await _cache.GetStringAsync(RedisKey.SMTP);

                if (string.IsNullOrEmpty(smtpCache))
                {
                    var resutl = await _smtpRepo.GetAll();
                    var jsonSmtp = JsonSerializer.Serialize(resutl.FirstOrDefault());
                    await _cache.SetStringAsync(RedisKey.SMTP, jsonSmtp, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(60)
                    });
                }
                else
                {
                    smtpEntity = JsonSerializer.Deserialize<SmtpEntity>(smtpCache);
                }
                return smtpEntity;

            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }

        }
    }
}
