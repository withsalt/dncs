using DNCS.Data.Entity.Content;
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
    [Table("User_Info")]
    public class UserInfo
    {
        [Key]
        [Required]
        [MaxLength(32)]
        public string UserID { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(60)]
        public string Email { get; set; }

        [MaxLength(18)]
        public string Phone { get; set; }

        [MaxLength(10)]
        public string Sex { get; set; }

        [MaxLength(18)]
        public string QQ { get; set; }

        [MaxLength(60)]
        public string Wechat { get; set; }

        /// <summary>
        /// 生日 时间戳
        /// </summary>
        public int? Brithday { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Logo { get; set; }

        /// <summary>
        /// 是否为VIP
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// 是否为开发人员
        /// </summary>
        public bool IsDeveloper { get; set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        public int UpdateTime { get; set; }

        public int CreateTime { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public virtual UserPasswd UserPasswd { get; set; }

        /// <summary>
        /// 登录历史
        /// </summary>
        public virtual ICollection<LoginHistory> LoginHistory { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<UserRoles> UserRoles { get; set; }

        /// <summary>
        /// 用户收藏的条目
        /// </summary>
        public virtual ICollection<UserCollection> UserCollection { get; set; }

        /// <summary>
        /// 条目
        /// </summary>
        public virtual ICollection<ContentInfo> ContentInfo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual ICollection<ContentType> ContentType { get; set; }
    }
}
