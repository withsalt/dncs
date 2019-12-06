using DNCS.Data.Entity.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNCS.Data.Entity.Content
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    [Table("Content_UserCollection")]
    public class UserCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(ContentInfo))]
        public int ContentId { get; set; }

        [Required]
        [MaxLength(32)]
        [ForeignKey(nameof(UserInfo))]
        public string UserID { get; set; }

        public int CreateTime { get; set; }

        public ContentInfo ContentInfo { get; set; }

        public UserInfo UserInfo { get; set; }

    }
}
