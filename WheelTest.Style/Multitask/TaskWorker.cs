using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Reflection;

namespace JSON.GTA.Tasks
{
    public class TaskWorker
    {
        ManualResetEvent errorWaitEventHandler;
        GTATask cData;
        AutoResetEvent finishedEventHandler;
        /// <summary>
        /// 暂停
        /// </summary>
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public Thread thread;

        SendOrPostCallback downloadCompletionCallback;
        SendOrPostCallback downloadPausedCallback;
        SendOrPostCallback downloadDoNothingCallback;

        /// <summary>
        /// 创建一个CourseFile执行器，去执行下载和启动程序这样的工作
        /// </summary>
        /// <param name="data"></param>
        public TaskWorker(GTATask data)
        {
            this.cData = data;
            data.Worker = this;
            this.errorWaitEventHandler = new ManualResetEvent(false);
            this.InitilizeSendorPostCallback();
        }

        /// <summary>
        /// 下载函数模块入口
        /// </summary>
        public void WorkEntry(AutoResetEvent FinishedEventHandler)
        {
            Exception occurEx = null;
            try
            {
                this.thread = Thread.CurrentThread;
                this.finishedEventHandler = FinishedEventHandler;
                this.finishedEventHandler.Reset();
                cData.State = TaskState.Running;
                cData.Excute();
                onFinished();
            }
            catch (System.Threading.ThreadAbortException abortEx)
            {
                occurEx = abortEx;
                this.errorWaitEventHandler.Set(); 
            }
            catch (Exception ex)
            {
                occurEx = ex;
                System.Diagnostics.Debug.WriteLine(ex.GetType().ToString());
                    this.cData.Message = HandErrorMessage(ex, null);
                    //System.IO.File.AppendAllText(@"d:\a.txt", HandStackTrace(ex,null));
                //发出信号，不再继续尝试（本地错误）
                this.errorWaitEventHandler.Set();
                //throw;
            }
            finally
            {
                if (occurEx != null)
                    this.OnTaskStop(null);
                else
                    this.OnTaskCompletion(null);
                cData.RaiseTaskCompleted(occurEx is ThreadAbortException?null:occurEx, occurEx != null);
            }
        }

        private string HandErrorMessage(Exception ex,StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (ex != null)
            {
                if (!(ex is System.Reflection.TargetInvocationException)&&!string.IsNullOrEmpty(ex.Message))
                {
                    sb.Append(ex.Message);
                    sb.Append(Environment.NewLine);
                }
                if (ex.InnerException != null)
                    HandErrorMessage(ex.InnerException, sb);
            }
            return sb.ToString();
        }

        private string HandStackTrace(Exception ex, StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (ex != null)
            {
                if (!(ex is System.Reflection.TargetInvocationException) && !string.IsNullOrEmpty(ex.StackTrace))
                {
                    sb.Append(ex.StackTrace);
                    sb.Append(Environment.NewLine);
                }
                if (ex.InnerException != null)
                    HandStackTrace(ex.InnerException, sb);
            }
            return sb.ToString();
        }

        private void onFinished()
        {
            this.cData.State = TaskState.Finish;

            //if (OnFileDownloaded != null)
            //{
            //    OnFileDownloaded(this.cData);
            //}
        }

        #region 异步操作中的CallBack及其相关的操作
        private void InitilizeSendorPostCallback()
        {
            this.downloadCompletionCallback = new SendOrPostCallback(this.OnTaskCompletion);
            this.downloadPausedCallback = new SendOrPostCallback(this.OnTaskStop);
            this.downloadDoNothingCallback = new SendOrPostCallback(this.OnTaskProgress);
        }

        /// <summary>
        /// 运行在窗体线程
        /// </summary>
        /// <param name="obj"></param>
        private void OnTaskCompletion(object obj)
        {
            //TODO:停止计时器
            //如果下载结束,开始解压缩
            this.cData.Worker = null;
            this.cData.State = TaskState.Finish;
            this.finishedEventHandler.Set();
        }

        /// <summary>
        /// 运行在窗体线程，由async调用这个方法
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        private void OnTaskStop(object obj)
        {

            //make sure that reponse is closeed.
            //清空RunningWorker
            this.cData.Worker = null;
            this.cData.State = TaskState.Stop;
            //flush to storage.
            //通告线程
            this.finishedEventHandler.Set();
            //TODO:停止计时器
        }

        /// 运行在窗体线程，由async调用这个方法
        /// </summary>
        /// <param name="obj"></param>
        private void OnTaskProgress(object obj)
        {
            //do nothing
            //      this.cData.OnStatueChange(null);
        }
        #endregion

        /// <summary>
        /// 停止正在运行的下载任务
        /// </summary>
        public void Stop()
        {
            //如果thread为空，说明没有对应线程在运行
            if (this.thread == null || !this.thread.IsAlive)
            {
                return;
            }

            this.cData.State = TaskState.Stop;

            this.errorWaitEventHandler.Set();
            _pauseEvent.Set();
            if (thread != null)
            {
                thread.Abort();
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            this.cData.State = TaskState.Pause;
            _pauseEvent.Reset();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            this.cData.State = TaskState.Running;
            _pauseEvent.Set();
        }

        /// <summary>
        /// 检查是否暂停
        /// </summary>
        internal void CheckIsPause()
        {
            _pauseEvent.WaitOne(Timeout.Infinite);
        }

        public void Restart()
        {
            cData.State = TaskState.Queue;
            cData.Worker = null;
            TaskEngine.AddTask(cData);
        }
    }
}
