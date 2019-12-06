using DNCS.Data.Model.Common.JsonObjectNode;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Data.Model.Request.Content
{
    public class ContentInfoParam : IChild
    {
        public int Id { get; set; }


        public string Content { get; set; }

        /// <summary>
        /// 鸡汤类型
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 出处
        /// </summary>
        public string Source { get; set; }

        public string CreateUser { get; set; }

        public bool IsTop { get; set; }
    }
}
