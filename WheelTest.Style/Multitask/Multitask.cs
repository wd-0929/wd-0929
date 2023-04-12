using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using JSON.GTA.Tasks;

namespace WheelTest.Style.Multitask
{


    public interface IMultitask
    {
        string Title { get; set; }

        int Count { get; }

        int FinishCount { get; }

        string Remark1 { get; set; }
    }


    public class Multitask<TParam, TReust> : IMultitask, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task[] _tasks;
        private Func<TParam, TReust> _doWork;
        private int _index;
        private GTATask gTATask;

        public Multitask(int taskCount, TParam[] datas, Func<TParam, TReust> doWork)
        {
            Init(taskCount, datas, doWork, null);
        }

        public Multitask(int taskCount, TParam[] datas, Func<TParam, TReust> doWork, GTATask task)
        {
            Init(taskCount, datas, doWork, task);
        }

        private void Init(int taskCount, TParam[] datas, Func<TParam, TReust> doWork, GTATask task)
        {
            if (taskCount <= 0) throw new ArgumentOutOfRangeException(nameof(taskCount));
            if (datas == null || datas.Length == 0) throw new ArgumentNullException(nameof(datas));

            _index = 0;

            DataInfo[] array = new DataInfo[datas.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new DataInfo(datas[i]);
            }
            _datas = new ReadOnlyCollection<DataInfo>(array);

            this._doWork = doWork;
            _cancellationTokenSource = new CancellationTokenSource();
            _tasks = new Task[Math.Min(taskCount, datas.Length)];

            gTATask = task;
            for (int i = 0; i < _tasks.Length; i++)
            {
                _tasks[i] = new Task(DoWork, _cancellationTokenSource.Token);
                _tasks[i].Start();
            }
        }

        private ReadOnlyCollection<DataInfo> _datas;

        public string Title
        {
            get;
            set;
        }

        public int FinishCount
        {
            get
            {
                return _datas.Count(d => d.Status != Status.Waiting);
            }
        }

        public int Count
        {
            get
            {
                return _datas.Count;
            }
        }

        public DataInfo[] FinishDatas
        {
            get
            {
                return _datas.Where(d => d.Status == Status.Finish).ToArray();
            }
        }

        public DataInfo[] ErrorDatas
        {
            get
            {
                return _datas.Where(d => d.Status == Status.Error).ToArray();
            }
        }

        public string Remark1
        {
            get;
            set;
        }

        private void DoWork()
        {
            while (true)
            {
                // todo 多任务暂停和终止

                if (gTATask != null)
                {
                    gTATask.CheckIsPause();

                    if (gTATask.State == TaskState.Stop)
                        this.Stop();
                }

                if (_cancellationTokenSource.IsCancellationRequested) break;
                DataInfo info = GetNext();
                if (info != null)
                {
                    try
                    {
                        info.Reulst = _doWork(info.Param);
                        info.Status = Status.Finish;
                    }
                    catch (Exception ex)
                    {
                        info.Error = ex;
                        info.Status = Status.Error;
                    }
                    finally
                    {
                        StatusChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                    break;
            }
        }

        private DataInfo GetNext()
        {
            lock (this)
            {
                if (_index == _datas.Count)
                {
                    return null;
                }
                else
                {
                    DataInfo info = _datas[_index];
                    _index++;
                    return info;
                }
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        public void WaitAll()
        {
            Task.WaitAll(_tasks);
        }

        public void Dispose()
        {
            foreach (var item in _tasks)
            {
                item.Dispose();
            }
        }

        public event EventHandler StatusChanged;

        public class DataInfo
        {
            public DataInfo(TParam param)
            {
                this.Param = param;
                this.Status = Status.Waiting;
            }

            public TParam Param { get; private set; }

            public TReust Reulst { get; set; }

            public Status Status { get; set; }

            public Exception Error { get; set; }
        }

        public enum Status
        {
            Waiting,
            Finish,
            Error,
        }
    }
}
