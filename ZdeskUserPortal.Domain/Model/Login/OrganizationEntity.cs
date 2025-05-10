using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Login
{
    [Table("Zdesk_m_organization_Info_tbl")]
    public class OrganizationEntity
    {
        public string? Logo { get; set; } = string.Empty;
    }
}
