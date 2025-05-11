using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.Domain.RepositoryInterfaces.Login;
using ZdeskUserPortal.Utility;
using ZdeskUserPortal.Utility.HtmlTemplate;

namespace ZdeskUserPortal.Business.Services
{
    public class EmailTemplateServices : IEmailTemplate
    {
        private readonly IMaster _masterRepo;

        public EmailTemplateServices(IMaster masterRepo)
        {
            _masterRepo = masterRepo;
        }

        public async Task<string> GetHtmlTemplate(string htmlTemplate, Dictionary<string, string> parameters)
        {
            var organizationInfo = await GetOrganizationInfo();
            return  TemplateHelper.GetTemplate(htmlTemplate, parameters, organizationInfo);
        }

        public async Task<EmailTemplateEntity> GetEmailTemplate(string templateName)
        {
            try
            {
                return await _masterRepo.EmailTemplateDetail(templateName);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
        }

        private async Task<Dictionary<string, string>> GetOrganizationInfo()
        {
            var organizationInfo = await _masterRepo.OrganizationDetail();
            var result = new Dictionary<string, string>()
            {
                {"image",organizationInfo.Logo },
                {"OrgName",organizationInfo.OrganizationName },
                {"Address",organizationInfo.Address },
                {"Email",organizationInfo.support_email },
            };

            return result;

        }
       
    }
}
