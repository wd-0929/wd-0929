using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace JSON.GTA.Tasks
{
    public class GTATaskCollection : ObservableCollection<GTATask>
    {
        private SynchronizationContext currentSynchronizationContext;
        public GTATaskCollection()
        {
            currentSynchronizationContext = SynchronizationContext.Current;
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object addItem in e.NewItems)
                {
                    GTATask addTask = addItem as GTATask;
                    if (!(addTask.State == TaskState.Stop || addTask.State == TaskState.Finish || addTask.State == TaskState.Removed))
                    {
                        HasTaskRuning = true;
                        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("HasTaskRuning"));
                    }
                    addTask.TaskStateChanged += new EventHandler<TaskStateChangeEventArgs>(addTask_TaskStateChanged);
                }
            }
        }

        void addTask_TaskStateChanged(object sender, TaskStateChangeEventArgs e)
        {
            //if (e.State == TaskState.Running)
            //{
            //    HasTaskRuning = true;
            //    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("HasTaskRuning"));
            //    Debug.WriteLine("任务启动");
            //}
            if (Count > 0)
            {
                foreach (GTATask task in base.Items)
                {
                    if (!(task.State == TaskState.Stop || task.State == TaskState.Finish || task.State == TaskState.Removed))
                    {
                        HasTaskRuning = true;
                        break;
                    }
                    else
                        HasTaskRuning = false;
                }
            }
            else
                HasTaskRuning = false;

            if (HasTaskRuning)
                Debug.WriteLine("有任务在运行中");
            else
                Debug.WriteLine("没有任务在运行");

            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("HasTaskRuning"));
            currentSynchronizationContext.Send(ResetCollection, null);

        }

        private void ResetCollection(object obj)
        {
            base.OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        public bool HasTaskRuning
        {
            get;
            set;
        }

        void addTask_TaskCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }

        public GTATask this[Guid taskID]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    GTATask task = this[i];
                    if (task.TaskID.CompareTo(taskID) == 0)
                    {
                        return task;
                    }
                }
                return null;
            }
        }
    }
}