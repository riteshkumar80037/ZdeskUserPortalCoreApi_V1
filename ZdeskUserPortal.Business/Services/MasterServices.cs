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
        private readonly IBaseRepository<EmailTemplateEntity> _emailTemplateRepo;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public MasterServices(IBaseRepository<BusinessUnitEntity> businessUnitRepository,
            IBaseRepository<OrganizationEntity> organizationRepository, IDistributedCache cache,
            IMapper mapper, IBaseRepository<SmtpEntity> smtpRepo, IBaseRepository<EmailTemplateEntity> emailTemplateRepo)
        {
            _businessUnitRepo = businessUnitRepository;
            _organazationRepo = organizationRepository;
            _cache = cache;
            _mapper = mapper;
            _smtpRepo = smtpRepo;
            _emailTemplateRepo = emailTemplateRepo;
        }

        public async Task<EmailTemplateEntity> EmailTemplateDetail(string templateName)
        {
            EmailTemplateEntity emailTemplateEntity = new EmailTemplateEntity();
            try
            {

                var emailTemplateCache = await _cache.GetStringAsync(RedisKey.EMAILTEMPLATE);

                if (string.IsNullOrEmpty(emailTemplateCache))
                {
                    var resutl = await _emailTemplateRepo.GetAll(x => x.Active == true);
                    emailTemplateEntity = resutl.Where(x => x.TemplateName == templateName).FirstOrDefault();
                    var jsonSmtp = JsonSerializer.Serialize(resutl);
                    await _cache.SetStringAsync(RedisKey.EMAILTEMPLATE, jsonSmtp, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(60)
                    });
                }
                else
                {
                    var emailTemplateResult = JsonSerializer.Deserialize<IEnumerable<EmailTemplateEntity>>(emailTemplateCache);
                    emailTemplateEntity=emailTemplateResult.Where(x=>x.TemplateName== templateName).FirstOrDefault();
                 }
                return emailTemplateEntity;

            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }

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

        public async Task<OrganizationEntity> OrganizationDetail()
        {
            OrganizationEntity organizationEntity = null;
            try
            {
                
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
                return organizationEntity;


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
                    var resutl = await _smtpRepo.GetAll(x=>x.Active==true);
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
