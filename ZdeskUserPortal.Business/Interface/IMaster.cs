using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business.Interface
{
    public interface IMaster
    {
        Task<IEnumerable<BusinessUnitEntity>> GetAllBusinessUnit();
        Task<LogoDTO> OrganizationDetail();
        Task<SmtpEntity> SmtpDetail();

    }
}
