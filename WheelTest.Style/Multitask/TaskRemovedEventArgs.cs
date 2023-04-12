using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    public class TaskRemovedEventArgs:EventArgs
    {
                /// <summary>
        /// 任务移除事件参数
        /// </summary>
        /// <param name="task">需要移除的任务</param>
        public TaskRemovedEventArgs(GTATask task)
        {
            this.RemoveTask = task;
        }

        /// <summary>
        /// 需要移除的任务
        /// </summary>
        public GTATask RemoveTask { get; set; }
    }
}
