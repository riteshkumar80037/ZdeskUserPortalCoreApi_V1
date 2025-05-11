using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Business.Services;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business.Interface
{
    public interface IMaster
    {
        Task<IEnumerable<BusinessUnitEntity>> GetAllBusinessUnit();
        Task<OrganizationEntity> OrganizationDetail();
        Task<SmtpEntity> SmtpDetail();
        Task<EmailTemplateEntity> EmailTemplateDetail(string templateName);

    }
}
