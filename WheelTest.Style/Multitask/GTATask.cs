using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace JSON.GTA.Tasks
{
    public class GTATask : IDisposable, INotifyPropertyChanged
    {
        public static AsyncOperation asyncOperation;
        ///// <summary>
        ///// 任务
        ///// </summary>
        ///// <param name="ownObjType">静态方法所在的类</param>
        ///// <param name="methodName">方法名</param>
        ///// <param name="addWorkPara">是否自动在参数最后添加TaskWorker参数</param>
        ///// <param name="supportPause">是否支持暂停</param>
        ///// <param name="titleName">任务标题名</param>
        ///// <param name="paras">参数</param>
        //[Obsolete("此方法已经过期")]
        //public GTATask(string titleName, Type ownObjType, string methodName, bool addWorkPara = true, bool supportPause = true, params object[] paras)
        //{
        //    Tasks.Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
        //    if (asyncOperation == null)
        //        asyncOperation = AsyncOperationManager.CreateOperation("Global");

        //    //this.Worker= new TaskWorker(this);
        //    SetMethodParas(addWorkPara, paras);
        //    this.addWorkPara = addWorkPara;
        //    this.ownObjType = ownObjType;
        //    this.methodName = methodName;
        //    this.Title = titleName;
        //    taskID = Guid.NewGuid();
        //    this.SupportPause = supportPause;
        //}

        ///// <summary>
        ///// 任务
        ///// </summary>
        ///// <param name="ownObj">执行方法的实例</param>
        ///// <param name="methodName">方法名</param>
        ///// <param name="addWorkPara">是否自动在参数最后添加TaskWorker参数</param>
        ///// <param name="titleName">任务标题名</param>
        ///// <param name="paras">参数</param>
        //[Obsolete("此方法已经过期")]
        //public GTATask(string titleName, object ownObj, string methodName, bool addWorkPara = true, bool supportPause = true, params object[] paras)
        //    : this(titleName, ownObj.GetType(), methodName, addWorkPara, supportPause, paras)
        //{
        //    this.ownObj = ownObj;
        //}

        public GTATask(string titleName, Delegate invokeMethod, bool addWorkPara = true, bool supportPause = true, params object[] paras)
        {
            this.InvodeMethod = invokeMethod;

            Tasks.Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            if (asyncOperation == null)
                asyncOperation = AsyncOperationManager.CreateOperation("Global");

            //this.Worker= new TaskWorker(this);
            SetMethodParas(addWorkPara, paras);
            this.addWorkPara = addWorkPara;
            this.Title = titleName;
            taskID = Guid.NewGuid();
            this.SupportPause = supportPause;
        }

        #region 任务执行
        Delegate InvodeMethod = null;
        private bool addWorkPara;
        /// <summary>
        /// 该执行方法的类实力
        /// </summary>
        private object ownObj = null;

        private Type ownObjType = null;

        private string methodName = null;

        /// <summary>
        /// 方法所需要的参数
        /// </summary>
        object[] paras = null;
        Type[] paraTypes = null;

        private MethodBase method;

        /// <summary>
        /// 任务执行者
        /// </summary>
        public TaskWorker Worker
        {
            get;
            internal set;
        }

        internal MethodBase GetExcuteMethod()
        {
            if (method == null)
            {
                method = ownObjType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, GetMethodParaTypes(), null);
            }
            return method;
        }

        /// <summary>
        /// 设置任务的执行参数
        /// </summary>
        /// <param name="addWorkPara"></param>
        /// <param name="paras"></param>
        public void SetMethodParas(bool addWorkPara, params object[] paras)
        {
            IList<object> rparas = new List<object>();
            if (paras != null)
            {
                foreach (object p in paras)
                {
                    rparas.Add(p);
                }
            }
            if (addWorkPara)
                rparas.Add(this);

            if (rparas.Count > 0)
                this.paras = rparas.ToArray<object>();
            else
                this.paras = null;
        }

        /// <summary>
        /// 添加任务的执行参数
        /// </summary>
        /// <param name="para">添加的参数</param>
        /// <param name="paras"></param>
        public void AddMethodParas(object para)
        {
            IList<object> rparas = new List<object>();
            if (paras != null && paras.Length > 0)
            {
                for (int i = 0; i < paras.Length; i++)
                {
                    if (i == paras.Length - 1)
                    {
                        if (paras[i] == this)
                        {
                            continue;
                        }
                    }
                    rparas.Add(paras[i]);
                }
            }

            rparas.Add(para);
            if (addWorkPara)
                rparas.Add(this);
            if (rparas.Count > 0)
                this.paras = rparas.ToArray<object>();
            else
                this.paras = null;
        }

        /// <summary>
        /// 获取方法的参数类型集合
        /// </summary>
        /// <returns>返回方法参数类型的集合</returns>
        internal Type[] GetMethodParaTypes()
        {
            if (paraTypes == null)
            {
                if (paras == null || paras.Length <= 0)
                    paraTypes = Type.EmptyTypes;
                else
                {
                    paraTypes = new Type[paras.Length];
                    for (int i = 0; i < paras.Length; i++)
                    {
                        paraTypes[i] = paras[i].GetType();
                    }

                }
            }
            return paraTypes;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <returns>返回方法的返回值</returns>
        public object Excute()
        {
            if (InvodeMethod == null)
            {
                if (GetExcuteMethod() == null)
                    throw new TaskException(new NotFoundMethodException(ownObjType, methodName));

                if (ownObj == null)
                    ReturnValue = method.Invoke(null, paras);
                else
                    ReturnValue = method.Invoke(ownObj, paras);
                return ReturnValue;
            }
            else
            {
                ReturnValue = InvodeMethod.DynamicInvoke(paras);
                return ReturnValue;

            }
        }

        /// <summary>
        /// 方法的返回值，如没有返回值，则为null
        /// </summary>
        public object ReturnValue
        {
            get;
            set;
        }
        #endregion

        #region 事件
        private EventHandlerList events;

        protected EventHandlerList Events
        {
            get
            {
                if (this.events == null)
                {
                    this.events = new EventHandlerList();
                }
                return this.events;
            }
        }

        public event RunWorkerCompletedEventHandler TaskCompleted;

        internal void RaiseTaskCompleted(Exception ex, bool iscanceled)
        {
            asyncOperation.Post(new SendOrPostCallback((object obj) =>
                {
                    if (TaskCompleted != null)
                    {
                        TaskCompleted(this, new RunWorkerCompletedEventArgs(ReturnValue, ex, iscanceled));
                    }
                }), this);
        }

        public delegate void TaskRemovingEventHandler(object sender, TaskRemovingEventArgs e);
        /// <summary>
        /// 任务移除前
        /// </summary>
        public event TaskRemovingEventHandler TaskRemoving;

        public delegate void TaskRemovedEventHandler(object sender, TaskRemovedEventArgs e);
        /// <summary>
        /// 任务移除
        /// </summary>
        public event TaskRemovedEventHandler TaskRemoved;

        /// <summary>
        /// 任务状态改变
        /// </summary>
        public event EventHandler<TaskStateChangeEventArgs> TaskStateChanged;

        /// <summary>
        /// 任务单击
        /// </summary>
        public delegate void OnTaskClickDelegate(GTATask task, params object[] paras);

        private OnTaskClickDelegate taskClick = null;
        /// <summary>
        /// 任务单击
        /// </summary>
        public OnTaskClickDelegate TaskClick
        {
            get
            {
                return taskClick;
            }
            set
            {
                taskClick = value;
                OnPropertyChanged("CanClick");
            }
        }

        public void OnTaskClick()
        {
            if (TaskClick != null)
            {
                TaskClick(this, paras);
            }
        }
        #endregion

        #region 属性
        private Guid taskID = Guid.Empty;
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskID
        {
            get
            {
                return taskID;
            }
            set
            {
                taskID = value;
            }
        }

        private string title = Tasks.TaskName;
        /// <summary>
        /// 任务标题
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public object Data;

        public event EventHandler FailureDetails;

        public void OnTFailureDetailsClick()
        {
            if (FailureDetails != null)
            {
                FailureDetails(Data, EventArgs.Empty);
            }
        }

        #region IsVisibility Property
        private bool _isVisibility;
        /// <summary>
        /// 是否显示查看失败详情
        /// </summary>
        public bool IsVisibility
        {
            get
            {
                return _isVisibility;
            }
            set
            {
                if (_isVisibility != value)
                {
                    _isVisibility = value;
                    OnPropertyChanged(nameof(IsVisibility));
                }
            }
        }
        #endregion IsVisibility Property

        private uint percentProgress = 0;
        /// <summary>
        /// 进度
        /// </summary>
        public uint PercentProgress
        {
            get
            {
                return percentProgress;
            }
            set
            {
                percentProgress = value;
                OnPropertyChanged("PercentProgress");
            }
        }

        private uint total;
        /// <summary>
        /// 总数
        /// </summary>
        public uint Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        private bool supportPause = true;
        /// <summary>
        /// 是否支持停止
        /// </summary>
        public bool SupportPause
        {
            get
            {
                return supportPause;
            }
            set
            {
                supportPause = value;
                OnPropertyChanged("SupportPause");
                OnPropertyChanged("CanPause");
            }
        }

        private bool supportRemove = true;
        public bool SupportRemove
        {
            get
            {
                return supportRemove;
            }
            set
            {
                supportRemove = value;
                OnPropertyChanged("SupportRemove");
                OnPropertyChanged("CanRemove");
            }
        }

        private TaskState state = TaskState.Queue;
        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnTaskStateChanged(state);
                }
                OnPropertyChanged("State");
                OnPropertyChanged("CanPause");
                OnPropertyChanged("CanStop");
                OnPropertyChanged("CanContinue");
                OnPropertyChanged("CanRestart");
                OnPropertyChanged("CanRemove");
            }
        }

        private void OnTaskStateChanged(TaskState state)
        {
            if (TaskStateChanged != null)
            {
                TaskStateChangeEventArgs eventArgs = new TaskStateChangeEventArgs(this);
                TaskStateChanged(this, eventArgs);
            }
        }

        private string message;
        /// <summary>
        /// 信息
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (string.Compare(message, value, false) != 0)
                {
                    message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        /// <summary>
        /// 获取是否可以停止
        /// </summary>
        public bool CanPause
        {
            get
            {
                return SupportPause && state == TaskState.Running;
            }
        }

        /// <summary>
        /// 获取是否可以停止
        /// </summary>
        public bool CanStop
        {
            get
            {
                return state == TaskState.Running || state == TaskState.Pause;
            }
        }

        public bool CanRemove
        {
            get
            {
                if (!supportRemove)
                {
                    return state == TaskState.Finish || state == TaskState.Stop;
                }
                else
                    return state != TaskState.Removed && (state == TaskState.Queue || state == TaskState.Finish || state == TaskState.Stop);
            }
        }

        /// <summary>
        /// 获取是否可以继续，只有暂停后才能继续
        /// </summary>
        public bool CanContinue
        {
            get
            {
                return state == TaskState.Pause;
            }
        }

        /// <summary>
        /// 获取是否可以重试
        /// </summary>
        public bool CanRestart
        {
            get
            {
                return state == TaskState.Finish || state == TaskState.Stop;
            }
        }

        /// <summary>
        /// 关联数据
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可以单击
        /// </summary>
        public bool CanClick
        {
            get
            {
                return TaskClick != null;
            }
        }
        #endregion

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (CanPause)
                Worker.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            if (CanContinue)
                Worker.Continue();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop(bool enforce = false)
        {
            if (CanStop || enforce)
            {
                TaskEngine.PauseDownloadEngine(false);

                PercentProgress = 0;
                Worker.Stop();

                TaskEngine.ResumeDownloadEngine();

            }
        }

        /// <summary>
        /// 重试
        /// </summary>
        public void Restart()
        {
            if (CanRestart)
            {
                Total = 0;
                PercentProgress = 0;
                Message = string.Empty;
                this.State = TaskState.Queue;
                this.Worker = null;
                TaskEngine.AddTask(this);
            }
        }

        public void Remove()
        {
            if (CanRemove)
            {
                TaskRemovingEventArgs removingEventArgs = new TaskRemovingEventArgs(this);
                if (TaskRemoving != null)
                    TaskRemoving(this, removingEventArgs);

                if (!removingEventArgs.Cancel)
                {
                    Stop();

                    this.State = TaskState.Removed;

                    if (TaskRemoved != null)
                        TaskRemoved(this, new TaskRemovedEventArgs(this));
                }
            }
        }

        public void Dispose()
        {
            ownObj = null;
            ownObjType = null;
            methodName = null;
            method = null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CheckIsPause()
        {
            if (this.Worker != null)
                this.Worker.CheckIsPause();
        }
    }
}
