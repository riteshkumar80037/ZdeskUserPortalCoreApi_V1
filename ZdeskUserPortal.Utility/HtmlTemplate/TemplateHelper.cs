using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Master;

namespace ZdeskUserPortal.Utility.HtmlTemplate
{
    public static class TemplateHelper
    {
       
        public static string GetTemplate(string htmlTemplate,Dictionary<string,string> parameters, Dictionary<string, string> orgnizationInfo)
        {
            if (string.IsNullOrEmpty(htmlTemplate) || parameters == null)
                return htmlTemplate;

            foreach(var pair in parameters)
            {
                string placeHolder = $"$${pair.Key}$$";
                htmlTemplate = htmlTemplate.Replace(placeHolder, pair.Value ?? string.Empty);
            }
            foreach (var pair in orgnizationInfo)
            {
                string placeHolder = $"$${pair.Key}$$";
                htmlTemplate = htmlTemplate.Replace(placeHolder, pair.Value ?? string.Empty);
            }
            return htmlTemplate;
        }
    }
}
