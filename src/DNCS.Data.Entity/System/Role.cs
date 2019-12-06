using DNCS.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNCS.Data.Entity.System
{
    [Table("Sys_Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Describe { get; set; }

        public int CreateTime { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
