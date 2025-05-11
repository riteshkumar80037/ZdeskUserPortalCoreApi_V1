using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Master
{
    [Table("zdesk_m_users_tbl")]
    public class UsersEntity : BaseEntity
    {
        [Column("user_id_pk")]
        [Key]
        public override int Id { get; set; } = 0;

        [Column("created_date")]
        public override DateTime? CreatedDate { get; set; }
        [NotMapped]
        public virtual int? CreatedBy { get; set; } = 0;
        [Column("updated_date")]
        [NotMapped]
        public virtual DateTime? UpdatedDate { get; set; }
        [Column("updated_by")]
        [NotMapped]
        public virtual int? UpdatedBy { get; set; } = 0;
        [Column("is_active")]
        public virtual bool? Active { get; set; } = false;
        public string Email { get; set; }
        [Column("user_code")]
        public string UserCode { get; set; }
        [Column("mobile_no")]
        public string MobileNumber { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
