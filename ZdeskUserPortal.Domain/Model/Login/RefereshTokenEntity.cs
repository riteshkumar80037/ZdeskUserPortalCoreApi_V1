using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdeskUserPortal.Domain.Model.Login
{
    [Table("RefereshToken")]
    public class RefereshTokenEntity
    {
        [Key]
        public int TokenId { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public bool Active { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
