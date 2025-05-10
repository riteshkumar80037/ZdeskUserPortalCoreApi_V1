using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Master
{
    [Table("zdesk_m_business_unit_tbl")]
    public class BusinessUnitEntity : BaseEntity
    {
        [Column("Business_Unit")]
        public string? BusinessUnitName { get; set; } = string.Empty;
        [Column("Business_unit_id_pk")]
        public override int Id { get; set; } = 0;

        [Column("created_date")]
        public override DateTime? CreatedDate { get; set; }
        [NotMapped]
        public virtual int? CreatedBy { get; set; } = 0;
        [Column("updated_date")]
        public virtual DateTime? UpdatedDate { get; set; }
        [Column("updated_by")]
        public virtual int? UpdatedBy { get; set; } = 0;
        [Column("is_active")]
        public virtual bool? Active { get; set; } = false;

    }
}
