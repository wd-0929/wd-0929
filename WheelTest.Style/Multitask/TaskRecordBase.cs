using JSON.GTA.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace JSON.GTA.Tasks
{
    [Serializable]
    public abstract class TaskRecordBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private const string ExtensionName = "dat";
        public TaskRecordBase(string title, bool isTask, TaskRecordData[] datas)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("title");
            Title = title;
            IsTask = isTask;
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            CanEdit = true;
            VerifyColumns = new List<string>();
            if (datas == null || datas.Length == 0) throw new ArgumentNullException("datas");
            Datas = datas;
            VerifyColumns.Add("Datas");
        }

        public TaskRecordBase()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            CanEdit = true;
            VerifyColumns = new List<string>();
            VerifyColumns.Add("Datas");
        }
        #region INotifyPropertyChanged
        [NonSerialized]
        private PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region CanEdit Property
        private bool _canEdit;

        public virtual bool CanEdit
        {
            get { return _canEdit; }
            private set
            {
                if (_canEdit != value)
                {
                    _canEdit = value;
                    OnPropertyChanged("CanEdit");
                }
            }
        }
        #endregion

        public Guid Id { get; private set; }

        public string Title { get; set; }

        public DateTime CreateTime { get; private set; }

        #region Message Property
        private string _message;

        public virtual string Message
        {
            get { return _message; }
            private set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
                if (_currentTask != null)
                {
                    _currentTask.Message = value;
                }
            }
        }
        #endregion

        public bool IsTask { get; private set; }

        [NonSerialized]
        private GTATask _currentTask;
        public void Run()
        {
            if (CanEdit)
            {
                CanEdit = false;
                if (IsTask)
                {
                    _currentTask = new GTATask(Title, new Action<GTATask>(Execute), true);
                    _currentTask.TaskClick = CreateTaskClick();
                    TaskEngine.AddTask(_currentTask);
                }
                else
                {
                    ThreadProxy.Execute<GTATask>(Execute, null);
                }
            }
        }

        protected GTATask.OnTaskClickDelegate CreateTaskClick()
        {
            if (CanView)
            {
                return new GTATask.OnTaskClickDelegate((task, arguments) =>
                {
                    View();
                });
            }
            else
            {
                return null;
            }
        }

        private string GetFileName()
        {
            string path = System.IO.Path.Combine(GetDirectoryPath(), CreateTime.ToString("yyyyMMdd"));
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return System.IO.Path.Combine(path, Id.ToString() + "." + ExtensionName);
        }

        public TaskRecordData[] Datas { get; private set; }

        protected int SuccessCount { get; private set; }

        protected virtual object[] GetExecuteSingleArguments()
        {
            return new object[0];
        }

        protected virtual bool ChcekIsLicense()
        {
            return true;
        }

        protected void Execute(GTATask task)
        {
            _currentTask = task;
            bool isLicense = ChcekIsLicense();
            if (isLicense)
            {
                Record();
                int index = 0;
                SuccessCount = 0;
                foreach (var item in Datas)
                {
                    if (task != null)
                        task.CheckIsPause();
                    index++;
                    Message = string.Format(Tasks.TaskRecordMessage, index, Datas.Length);
                    if (item.IsSelected)
                    {
                        if (item.Status != TaskItemStatus.Completed)
                        {
                            if (item.Execute(GetExecuteSingleArguments()))
                            {
                                SuccessCount++;
                            }
                            Record();
                        }
                    }
                    else
                    {
                        item.Status = TaskItemStatus.Ignore;
                        Record();
                    }
                }
            }
            OnCompleted(Datas.Count(tmp => tmp.IsSelected && tmp.Status != TaskItemStatus.Completed), isLicense);

        }

        protected abstract void View();

        public abstract bool CanView { get; }

        protected void Record()
        {
            if (IsTask)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (System.IO.FileStream stream = new System.IO.FileStream(GetFileName(), System.IO.FileMode.Create))
                {
                    formatter.Serialize(stream, this);
                }
            }
        }

        protected virtual void OnCompleted(int unfinishedCount, bool isLicense)
        {
            CanEdit = true;
            if (isLicense)
            {
                if (unfinishedCount == 0)
                {
                    Message = Tasks.TaskRecordCompletedMessage;
                    if (IsTask)
                    {
                        Delete();
                    }
                }
                else
                {
                    Message = string.Format(Tasks.TaskRecordCompletedButErrorMessage, unfinishedCount);
                    Record();
                }
            }
            else
            {
                Message = Tasks.NotAuthorization;
                Record();
            }
        }

        private void Delete()
        {
            System.IO.File.Delete(GetFileName());
        }

        protected void CheckIsPause()
        {
            if (IsTask && _currentTask != null)
            {
                _currentTask.CheckIsPause();
            }
        }

        public string Error
        {
            get { return string.Join(Environment.NewLine, VerifyColumns.Select(tmp => this[tmp]).Where(tmp => !string.IsNullOrWhiteSpace(tmp))); }
        }

        public string this[string columnName]
        {
            get { return Verify(columnName); }
        }

        protected readonly List<string> VerifyColumns;
        protected virtual string Verify(string columnName)
        {
            if (columnName == "Datas")
            {
                if (Datas.FirstOrDefault(tmp => tmp.IsSelected && tmp.Status != TaskItemStatus.Completed) == null)
                {
                    return Tasks.TaskRecordNoSelectedCanRunItems;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #region Static
        public static string GetDirectoryPath()
        {
            return "";
        }

        private static bool Read(string fileName, out TaskRecordBase data)
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
                    {
                        data = (TaskRecordBase)formatter.Deserialize(stream);
                        return true;
                    }
                }
            }
            catch { }
            data = null;
            return false;
        }

        public static TaskRecordBase[] ReadUnfinished()
        {
            string path = GetDirectoryPath();
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(GetDirectoryPath(), "*." + ExtensionName, SearchOption.AllDirectories);
                List<TaskRecordBase> datas = new List<TaskRecordBase>();
                TaskRecordBase data;
                foreach (var file in files)
                {
                    if (Read(file, out data))
                    {
                        data.CanEdit = true;
                        datas.Add(data);
                    }
                }
                return datas.ToArray();
            }
            else
            {
                return new TaskRecordBase[0];
            }
        }

        public static void ExecuteItem(object item)
        {
            ((TaskRecordBase)item).Run();
        }

        public static void DeleteItem(object item)
        {
            TaskRecordBase record = (TaskRecordBase)item;
            record.Delete();
        }

        public static void ViewItem(object item)
        {
            TaskRecordBase record = (TaskRecordBase)item;
            if (record.CanView)
                record.View();
        }

        #endregion Static
    }


    [Serializable]
    public abstract class TaskRecordData : INotifyPropertyChanged
    {
        public TaskRecordData()
        {
            Status = TaskItemStatus.Waitting;
            IsSelected = true;
        }

        #region INotifyPropertyChanged
        [NonSerialized]
        private PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region IsSelected Property
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }
        #endregion

        #region Status Property
        private TaskItemStatus _status;
        /// <summary>
        /// 状态
        /// </summary>
        public TaskItemStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        #endregion

        #region Message Property
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }
        #endregion

        public bool Execute(object[] parameters)
        {
            if (Status != TaskItemStatus.Completed)
            {
                Status = TaskItemStatus.Running;
                try
                {
                    if (OnExecute(parameters))
                    {
                        Status = TaskItemStatus.Completed;
                        Message = Tasks.TaskRecordDataExecuteSuccess;
                    }
                    else
                    {
                        Status = TaskItemStatus.Error;
                        Message = Tasks.TaskRecordDataExecuteFail;
                    }
                }
                catch (Exception ex)
                {
                    Status = TaskItemStatus.Error;
                    Message = ex.Message;
                }
            }
            return Status == TaskItemStatus.Completed;
        }

        protected abstract bool OnExecute(object[] arguments);
    }

    public enum TaskItemStatus
    {
        Waitting,
        Running,
        Completed,
        Error,
        Ignore
    }


}
