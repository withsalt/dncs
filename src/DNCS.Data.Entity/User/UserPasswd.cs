using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNCS.Data.Entity.User
{
    [Table("User_Password")]
    public class UserPasswd
    {
        [Key]
        [ForeignKey("UserInfo")]
        [MaxLength(32)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(256)]
        public string Token { get; set; }

        public int CreateTime { get; set; }

        public int UpdateTime { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
