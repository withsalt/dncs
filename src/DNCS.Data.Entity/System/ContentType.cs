using DNCS.Data.Entity.Content;
using DNCS.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DNCS.Data.Entity.System
{
    [Table("Sys_ContentType")]
    public class ContentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Describe { get; set; }

        [Required]
        [MaxLength(32)]
        [ForeignKey(nameof(UserInfo))]
        public string CreateUser { get; set; }

        public int CreateTime { get; set; }

        [MaxLength(32)]
        public string UpdateUser { get; set; }

        public int UpdateTime { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public UserInfo UserInfo { get; set; }

        public virtual ICollection<ContentInfo> ContentInfo { get; set; }
    }
}
