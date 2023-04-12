using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    public class AddedTaskEventArgs:EventArgs
    {
        public AddedTaskEventArgs(GTATask addedTask)
        {
            this.AddedTask = addedTask;
        }

        public GTATask AddedTask { get; set; }
    }
}
