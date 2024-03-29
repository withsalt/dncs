﻿namespace DNCS.Data.Model.Common.JsonObjectNode
{
    public interface IRoot<T> where T : IChild
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// 错误消息 自定义
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 自定义数据 需要重载Data
        /// </summary>
        T Data { get; set; }

        /// <summary>
        /// JSON创建时间/请求时间
        /// </summary>
        int Time { get; set; }

        /// <summary>
        /// JSON校验 默认5
        /// </summary>
        int Check { get; set; }
    }
}
