using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// 排队中
        /// </summary>
        Queue,
        /// <summary>
        /// 运行
        /// </summary>
        Running,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 停止
        /// </summary>
        Stop,
        /// <summary>
        /// 移除
        /// </summary>
        Removed,
        /// <summary>
        /// 完成
        /// </summary>
        Finish
    }
}
