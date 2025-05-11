using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Master;

namespace ZdeskUserPortal.Business.Interface
{
    public interface IEmailTemplate
    {
        Task<EmailTemplateEntity> GetEmailTemplate (string templateName);
        Task<string> GetHtmlTemplate(string htmlTemplate, Dictionary<string, string> parameters);
    }
}
