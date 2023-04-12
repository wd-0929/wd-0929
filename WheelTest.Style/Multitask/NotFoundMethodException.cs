using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON.GTA.Tasks
{
    /// <summary>
    /// 没有找到方法
    /// </summary>
    public class NotFoundMethodException : Exception
    {
        public NotFoundMethodException(Type ownType,string methodName)
        {
            this.OwnType = ownType;
            this.MethodName = methodName;
        }

        public Type OwnType
        {
            get;
            set;
        }

        public string MethodName
        {
            get;
            set;
        }
        private string messageFormat = Tasks.NotFoundMehtod;
        public override string Message
        {
            get
            {
                return string.Format(messageFormat, OwnType.FullName, MethodName);
            }
        }
    }
}
