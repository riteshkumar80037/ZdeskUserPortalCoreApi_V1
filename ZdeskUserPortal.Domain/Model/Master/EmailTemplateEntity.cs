using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Master
{
    [Table("zdesk_m_template_tbl")]
    public class EmailTemplateEntity
    {
        [Key]
        public int Id { get; set; }
        [Column("name")]
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        [Column("subject")]
        public string EmailSubject { get; set; }
        [Column("isactive")]
        public bool Active { get; set; }
    }
}
