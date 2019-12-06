using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNCS.Data.Entity.User
{
    [Table("User_LoginHistory")]
    public class LoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("UserInfo")]
        [MaxLength(32)]
        public string Uid { get; set; }

        [ForeignKey("LoginType")]
        public int TypeId { get; set; }

        [MaxLength(40)]
        public string LoginId { get; set; }

        public int LoginTime { get; set; }

        public virtual UserInfo UserInfo { get; set; }

    }
}
