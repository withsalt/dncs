using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Data.Model.Request.Content
{
    public class ContentTypeParam
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OldName { get; set; }

        public string Describe { get; set; }

        public string CreateUser { get; set; }

        public int CreateTime { get; set; }

        public string UpdateUser { get; set; }

        public int UpdateTime { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// 是否同步到微信
        /// </summary>
        public bool IsAsyncWechat { get; set; }
    }
}
