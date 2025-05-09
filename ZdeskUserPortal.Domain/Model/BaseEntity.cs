using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual int? CreatedBy { get; set; } = 0;
        public virtual DateTime? UpdatedDate { get; set; }
        public virtual int? UpdatedBy { get; set; } = 0;
        public virtual bool? Active { get; set; } = false;
    }
}
