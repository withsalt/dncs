using DNCS.Data.Model.Common.JsonObjectNode;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Data.Model.Response.Content
{
    public class ContentInfoModel: IChild
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int TypeId { get; set; }

        /// <summary>
        /// 鸡汤类型
        /// </summary>
        public int Type { get; set; }

        public string TypeName { get; set; }

        /// <summary>
        /// 出处
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadTime { get; set; }

        /// <summary>
        /// 是否置顶，插队发送
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否已经发送
        /// </summary>
        public bool HasSend { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public int SendTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public int CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

    }
}
