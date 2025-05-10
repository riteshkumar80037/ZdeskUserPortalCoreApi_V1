using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Login
{
    [Table("Zdesk_m_organization_Info_tbl")]
    public class OrganizationEntity:BaseEntity
    {

        [Key]
        public override int Id { get; set; } = 0;
        public string? Logo { get; set; } = string.Empty;
        public string? background { get; set; } = string.Empty;
        public string? OrganizationName { get; set; } = string.Empty;
        public string? ContactNo { get; set; } = string.Empty;
        public string? support_email { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        [NotMapped]
        public override DateTime? CreatedDate { get; set; }
        [NotMapped]
        public virtual int? CreatedBy { get; set; } = 0;
        [NotMapped]
        
        public virtual DateTime? UpdatedDate { get; set; }
        [NotMapped]
        
        public virtual int? UpdatedBy { get; set; } = 0;
        [NotMapped]
        public virtual bool? Active { get; set; } = false;
    }
}
