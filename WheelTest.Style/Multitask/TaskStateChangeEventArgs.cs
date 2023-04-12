using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    /// <summary>
    /// 任务状态改变事件参数
    /// </summary>
    public class TaskStateChangeEventArgs:EventArgs
    {
        /// <summary>
        /// 任务状态改变事件参数
        /// </summary>
        /// <param name="task">任务</param>
        public TaskStateChangeEventArgs(GTATask task)
        {
            this.Task = task;
        }

        /// <summary>
        /// 当前任务状态
        /// </summary>
        public TaskState State
        {
            get
            {
                return Task.State;
            }
        }

        /// <summary>
        /// 任务
        /// </summary>
        public GTATask Task { get; set; }
    }
}
