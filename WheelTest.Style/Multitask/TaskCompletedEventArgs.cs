using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    public class TaskCompletedEventArgs:EventArgs
    {
        public TaskCompletedEventArgs(GTATask completedTask)
        {
            this.CompletedTask = completedTask;
        }

        public GTATask CompletedTask { get; set; }
    }
}
