using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace JSON.GTA.Tasks
{
    /// <summary>
    /// 任务比较
    /// </summary>
    public class TaskEqualityComparer : IEqualityComparer<GTATask>
    {
        public bool Equals(GTATask x, GTATask y)
        {
            if (x.TaskID == y.TaskID)
                return true;
            else
                return false;
        }

        public int GetHashCode(GTATask obj)
        {
            return obj.GetHashCode();
        }
    }
}
