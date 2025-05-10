using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Master
{
    [Table("Zdesk_m_smpt_tbl")]
    public class SmtpEntity :BaseEntity
    {
    
        public string? ServiceProtocol { get; set; } = string.Empty;
        public string? FromAddress { get; set; } = string.Empty;
        public int? SmptPort { get; set; } = 0;
        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        [Column("Id_pk")]
        [Key]
        public override int Id { get; set; } = 0;

        [Column("CreatedDate")]
        public override DateTime? CreatedDate { get; set; }
        [NotMapped]
        public virtual int? CreatedBy { get; set; } = 0;
        [Column("updated_date")]
        [NotMapped]
        public virtual DateTime? UpdatedDate { get; set; }
        [Column("updated_by")]
        [NotMapped]
        public virtual int? UpdatedBy { get; set; } = 0;
        [Column("IsActive")]
        public virtual bool? Active { get; set; } = false;
    }
}
