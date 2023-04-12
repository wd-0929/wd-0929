using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    /// <summary>
    /// 任务移除前事件参数
    /// </summary>
    public class TaskRemovingEventArgs:EventArgs
    {
        /// <summary>
        /// 任务移除前事件参数
        /// </summary>
        /// <param name="task">需要移除的任务</param>
        public TaskRemovingEventArgs(GTATask task)
        {
            this.RemoveTask = task;
        }

        /// <summary>
        /// 移除的任务
        /// </summary>
        public GTATask RemoveTask { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
    }
}
