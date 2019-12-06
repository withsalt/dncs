using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNCS.Data.Entity.User
{
    /// <summary>
    /// 记录用户当天密码输入错误次数
    /// </summary>
    [Table("User_ValidateLog")]
    public class UserValidateLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 当天记录时间，Ex：20180910
        /// </summary>
        [MaxLength(8)]
        [Required]
        public string LogTime { get; set; }

        public int TryTime { get; set; }

        [MaxLength(60)]
        public string TryUid { get; set; }

        public int CreateTime { get; set; }
    }
}
