using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JSON.GTA.Tasks
{
    public class WorkerThread : WaitHandle
    {
        AutoResetEvent _finishedEvent;
        AutoResetEvent _closeEvent;
        ManualResetEvent _workerEvent;
        AutoResetEvent _hungryEvent;
        TaskWorker worker;
        Thread thread;
        bool iscontinue = true;

        public AutoResetEvent FinishedEvent
        {
            get { return _finishedEvent; }
        }

        public ManualResetEvent WorkerEvent
        {
            get { return _workerEvent; }
        }

        public AutoResetEvent HungryEvent
        {
            get { return _hungryEvent; }
        }

        public AutoResetEvent CloseEvent
        {
            get { return _closeEvent; }
        }

        public TaskWorker Worker
        {
            get { return worker; }
            set { worker = value; }
        }

        public WorkerThread()
        {
            this._workerEvent = new ManualResetEvent(true);//指示当前线呈是否可用
            this._hungryEvent = new AutoResetEvent(false);
            this._closeEvent = new AutoResetEvent(false);
            this._finishedEvent = new AutoResetEvent(true);
            thread = new Thread(new ThreadStart(ThreadEntry));
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        private void ThreadEntry()
        {
            while (true)
            {
                try
                {
                    _hungryEvent.WaitOne();
                    if (this.worker != null)
                    {
                        this.worker.WorkEntry(this._finishedEvent);
                    }
                }
                catch (ThreadAbortException e)
                {
                    if (e.ExceptionState is string)
                    {
                        this._closeEvent.Set();
                        return;
                    }
                    else
                    {
                        Thread.ResetAbort();
                    }
                }
                this.worker = null;

                this._workerEvent.Set();
                if (!iscontinue)
                {
                    this._finishedEvent.WaitOne();
                    this._closeEvent.Set();
                    return;
                }
            }
        }

        /// <summary>
        /// 关闭当前线程,同时关闭所有信号量资源
        /// </summary>
        public override void Close()
        {
            if (this.Worker != null)
            {
                lock (this)
                {
                    this.iscontinue = false;
                }
                this.Worker.Stop();
            }
            else
            {
                thread.Abort("STOP!!");
            }
            base.Close();
        }
    }
}
