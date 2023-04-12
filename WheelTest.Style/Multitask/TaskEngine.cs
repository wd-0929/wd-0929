using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

namespace JSON.GTA.Tasks
{
    public class TaskEngine
    {
        public static event EventHandler DownloadInvoked;
        public static event EventHandler BeforeDownloadInvoked;
        public static event EventHandler AfterDownloadInvoked;

        static Queue<GTATask> queue;
        static volatile GTATaskCollection tasks = new GTATaskCollection();
        static volatile List<WorkerThread> workers;
        static volatile List<EventWaitHandle> workerWaitHandles;
        static volatile List<EventWaitHandle> closeWaitHandles;
        static volatile int maxThreadNumber;
        static volatile int currentThreadNumber;
        static AsyncOperation async;
        static SendOrPostCallback doNothingCallback;
        static AutoResetEvent addNewThreadWorkerHandler;
        static ManualResetEvent pauseCheckHandler;
        static ManualResetEvent queueCheckHandler;
        static ManualResetEvent stopCheckHandler;
        static EventWaitHandle[] entryWaitHandler;
        static EventWaitHandle[] workerHandles;
        //static bool isInited = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Init()
        {
            //if (isInited)
            //{ return; }
            //isInited = true;
            doNothingCallback = new SendOrPostCallback(delegate (object obj) {; });
            //TaskWorker.asyncOperation.Post(new SendOrPostCallback(delegate(object obj)
            //{
            async = AsyncOperationManager.CreateOperation("DownloadEngine");
            //}), null);

            GTATask.asyncOperation = AsyncOperationManager.CreateOperation("Global");

            //用于排队的队列
            queue = new Queue<GTATask>();
            //存储工作线程
            workers = new List<WorkerThread>();
            //创建WaitHandle数组
            workerWaitHandles = new List<EventWaitHandle>();
            closeWaitHandles = new List<EventWaitHandle>();
            maxThreadNumber = 8;

            currentThreadNumber = 0;
            //register setting changed
            //创建addnewThreadWorkerHandler
            addNewThreadWorkerHandler = new AutoResetEvent(false);
            //
            pauseCheckHandler = new ManualResetEvent(true);
            //
            queueCheckHandler = new ManualResetEvent(false);
            //
            stopCheckHandler = new ManualResetEvent(true);
            //用户进入任务检查的WaitHandler
            entryWaitHandler = new EventWaitHandle[2];
            entryWaitHandler[0] = queueCheckHandler;
            entryWaitHandler[1] = stopCheckHandler;

            //添加新线程
            AddMoreThread();
            //设置网络参数
            //IReaper.Net.NetworkManager.SetConnectionLimit();
            //listen to setting change
            //开始监控线程
            Thread thread = new Thread(new ThreadStart(Update));
            thread.IsBackground = true;
            thread.Start();
        }


        #region 添加下载区域

        public static void BeginAddCourseFileData()
        {
            try
            {
                if (BeforeDownloadInvoked != null)
                    BeforeDownloadInvoked(null, EventArgs.Empty);
            }
            catch { }
        }

        /// <summary>
        /// 添加一个文件去下载
        /// </summary>
        /// <param name="Data"></param>
        public static void AddTask(GTATask Data)
        {
            if (Data == null)
                return;

            //设定判断条件，如果正在运行、正在排队，正在停止的则不能进入
            if (Data.State == TaskState.Finish || Data.Worker != null)
                return;

            //申请空间
            //if (Data.Storage.Index < 0)
            //{
            //    CourseFileDataManager.ApplyNewStorage(ref Data.Storage);
            //}

            lock (queue)
            {
                queue.Enqueue(Data);

                int index = tasks.IndexOf(Data);
                //加入用于绑定的集合里面
                if (index == -1)
                {
                    async.Post(new SendOrPostCallback((object obj) =>
                        {
                            tasks.Add(obj as GTATask);
                            Data.TaskRemoved += new GTATask.TaskRemovedEventHandler(Data_TaskRemoved);
                            Data.TaskCompleted += new RunWorkerCompletedEventHandler(Data_TaskCompleted);
                        }), Data);
                }
                if (TaskAdded != null)
                    TaskAdded(Data, new AddedTaskEventArgs(Data));
                //else
                //    tasks.Move(index, tasks.Count - 1);
                try
                {
                    if (DownloadInvoked != null)
                        DownloadInvoked(null, EventArgs.Empty);
                }
                catch { }
            }
            queueCheckHandler.Set();
            //如果不是大批量的导入，启动定时器
        }

        /// <summary>
        /// 任务添加事件
        /// </summary>
        public static event EventHandler<AddedTaskEventArgs> TaskAdded;

        /// <summary>
        /// 任务完成事件
        /// </summary>
        public static event EventHandler<TaskCompletedEventArgs> TaskCompleted;

        static void Data_TaskCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (TaskCompleted != null)
                TaskCompleted(sender, new TaskCompletedEventArgs(sender as GTATask));
        }

        static void Data_TaskRemoved(object sender, TaskRemovedEventArgs e)
        {
            tasks.Remove(e.RemoveTask);
        }

        public static void EndAddCourseFileData()
        {
            try
            {
                if (AfterDownloadInvoked != null)
                    AfterDownloadInvoked(null, EventArgs.Empty);
            }
            catch { }
        }
        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="Data"></param>
        public static void RemoveCourseFileData(GTATask Data)
        {
            //检查是否是
            Data.Worker.Stop();

            //CourseFileDataManager.InitFileData(ref Data);
        }

        #endregion

        #region 下载引擎控制区域
        /// <summary>
        /// 下载引擎的主运行区域
        /// </summary>
        static void Update()
        {
            GTATask data;
            workerHandles = new EventWaitHandle[workerWaitHandles.Count + 1];//.ToArray();
            workerHandles[0] = addNewThreadWorkerHandler;
            workerWaitHandles.CopyTo(workerHandles, 1);
            try
            {
                while (true)
                {
                    //空队循环
                    if (queue.Count == 0)
                    {
                        queueCheckHandler.Reset();
                        WaitHandle.WaitAll(entryWaitHandler);
                    }
                    else
                    {
                        WaitHandle.WaitAll(entryWaitHandler);
                        //等待一个workers可用
                        int index = WaitHandle.WaitAny(workerHandles);
                        //说明有新的线程需要被创建
                        //在workerHandler偏移量0的位置存放了一个用于通知管理者的信号
                        if (index == 0)
                        {
                            AddMoreThread();
                            workerHandles = new EventWaitHandle[workerWaitHandles.Count + 1];//.ToArray();
                            workerHandles[0] = addNewThreadWorkerHandler;
                            workerWaitHandles.CopyTo(workerHandles, 1);
                            continue;
                        }
                        //说明是一个正常的工作线程空闲
                        //将其恢复到其在工作线程列表中的位置（位置0是处理添加新线程的）
                        else
                        {
                            index--;
                        }
                        //检查暂停ManulResetEvent
                        pauseCheckHandler.WaitOne();
                        //如果当前线程数大于目标线程数
                        //就将当前醒来的线程关闭
                        if (currentThreadNumber > maxThreadNumber)
                        {
                            RemoveOneThread(index);
                            workerHandles = new EventWaitHandle[workerWaitHandles.Count + 1];//.ToArray();
                            workerHandles[0] = addNewThreadWorkerHandler;
                            workerWaitHandles.CopyTo(workerHandles, 1);
                            continue;
                        }
                        async.Post(doNothingCallback, null);
                        //取出对象，检查它的状态
                        data = queue.Dequeue();
                        //如果它是准备运行的，就运行之，否则就放弃了
                        if (data.State == TaskState.Removed || data.State != TaskState.Queue)
                        {
                            workerWaitHandles[index].Set();
                            continue;
                        }
                        else
                        {
                            workerWaitHandles[index].Reset();
                        }
                        //创建任务
                        TaskWorker worker = new TaskWorker(data);
                        workers[index].Worker = worker;
                        //唤醒工人
                        workers[index].HungryEvent.Set();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 暂停调度线程继续运行
        /// </summary>
        public static void PauseDownloadEngine(bool StopTask)
        {
            //禁止释放新任务
            stopCheckHandler.Reset();
            pauseCheckHandler.Reset();
            //停止
            if (StopTask)
            {
                foreach (WorkerThread thread in workers)
                {
                    if (thread.Worker != null)
                    {
                        thread.Worker.Stop();
                    }
                }
                EventWaitHandle.WaitAll(workerWaitHandles.ToArray());
            }
        }

        /// <summary>
        /// 继续调度线程运行
        /// </summary>
        public static void ResumeDownloadEngine()
        {
            stopCheckHandler.Set();
            pauseCheckHandler.Set();
        }
        /// <summary>
        /// 添加更多线程
        /// </summary>
        static void AddMoreThread()
        {
            int newThreadCount = maxThreadNumber - currentThreadNumber;
            if (newThreadCount < 0)
                return;
            lock (queue)
            {
                for (int i = 0; i < newThreadCount; i++)
                {
                    WorkerThread newThread = new WorkerThread();
                    workers.Add(newThread);
                    workerWaitHandles.Add(newThread.WorkerEvent);
                    closeWaitHandles.Add(newThread.CloseEvent);
                }
                currentThreadNumber = maxThreadNumber;
                addNewThreadWorkerHandler.Reset();
            }
        }

        /// <summary>
        /// 移除一个线程
        /// </summary>
        /// <param name="index"></param>
        static void RemoveOneThread(int index)
        {
            lock (queue)
            {
                //关闭资源
                workers[index].Worker = null;
                workers[index].Close();
                //删除workerWaitHandler中的记录
                workerWaitHandles.RemoveAt(index);
                //删除closeWaitHandler中的记录
                closeWaitHandles.RemoveAt(index);
                //删除workers中的记录
                workers.RemoveAt(index);
                //修改计数
                currentThreadNumber--;
            }
        }


        /// <summary>
        /// 关闭下载引擎
        /// </summary>
        public static void CloseTaskEngine()
        {
            //关闭
            if (async != null)
                async.OperationCompleted();
            //顺次关闭各个线程
            if (workers != null)
            {
                for (int i = 0; i < workers.Count; i++)
                {
                    workers[i].Close();
                }

                //WaitHandle.WaitAll(closeWaitHandles.ToArray());
                for (int i = 0; i < closeWaitHandles.Count; i++)
                {
                    closeWaitHandles[i].WaitOne();
                }
                for (int i = 0; i < workers.Count; i++)
                {
                    workers[i].CloseEvent.Close();
                    workers[i].FinishedEvent.Close();
                    workers[i].HungryEvent.Close();
                    workers[i].WorkerEvent.Close();
                }
            }
            //tp
        }
        #endregion

        public static GTATaskCollection Tasks
        {
            get { return tasks; }
        }
    }
}
