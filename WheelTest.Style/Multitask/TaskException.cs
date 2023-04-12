using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    /// <summary>
    /// 任务异常
    /// </summary>
    public class TaskException:Exception
    {
        public TaskException()
        {
 
        }

        public TaskException(Exception innerException)
            :base(null,innerException)
        {
 
        }

        public override string Message
        {
            get
            {
                return Tasks.TaskException;
            }
        }
    }
}
