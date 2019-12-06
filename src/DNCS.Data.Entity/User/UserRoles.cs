using DNCS.Data.Entity.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNCS.Data.Entity.User
{
    [Table("User_Roles")]
    public class UserRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(32)]
        [ForeignKey("UserInfo")]
        public string UserID { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual Role Role { get; set; }

    }
}
