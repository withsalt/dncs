using DNCS.Data.Entity.System;
using DNCS.Data.Entity.User;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNCS.Data.Entity.Content
{
    [Table("Content_Info")]
    public class ContentInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 鸡汤类型
        /// </summary>
        [ForeignKey(nameof(ContentType))]
        public int TypeId { get; set; }

        /// <summary>
        /// 出处
        /// </summary>
        [MaxLength(300)]
        public string Source { get; set; }

        /// <summary>
        /// 是否置顶，插队发送
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadTime { get; set; }

        /// <summary>
        /// 是否已经发送
        /// </summary>
        public bool HasSend { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [DefaultValue(0)]
        public int SendTime { get; set; }

        [Required]
        [MaxLength(32)]
        [ForeignKey(nameof(UserInfo))]
        public string CreateUser { get; set; }

        public int CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 用户收藏的条目
        /// </summary>
        public virtual ICollection<UserCollection> UserCollection { get; set; }

        public UserInfo UserInfo { get; set; }

        public ContentType ContentType { get; set; }
    }
}
