using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTest
{
    public class ThreadProxy
    {
        private readonly Delegate _method;
        private readonly object _tag;
        private readonly object[] _arguments;
        private readonly Action<ThreadProxyCallbackResult> _callbackMethod;

        private ThreadProxy(Delegate method, object[] arguments = null, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            if (method == null) throw new ArgumentNullException("method");
            _method = method;
            _tag = tag;
            _arguments = arguments;
            _callbackMethod = callbackMethod;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_arguments != null)
                {
                    e.Result = _method.DynamicInvoke(_arguments);
                }
                else
                {
                    e.Result = _method.DynamicInvoke();
                }
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            worker.DoWork -= worker_DoWork;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            worker.Dispose();
            worker = null;

            if (_callbackMethod != null)
            {
                if (e.Error == null)
                {
                    _callbackMethod(new ThreadProxyCallbackResult(_tag, e.Result));
                }
                else
                {
                    _callbackMethod(new ThreadProxyCallbackResult(_tag, e.Error));
                }
            }
        }

        #region Static Method

        public static void Execute(Action method, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, null, callbackMethod, tag);
        }
        public static void Execute<T>(Action<T> method, T arg, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg }, callbackMethod, tag);
        }
        public static void Execute<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4, arg5 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4, arg5, arg6 }, callbackMethod, tag);
        }

        public static void Execute<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, callbackMethod, tag);
        }

        public static void Execute<TResult>(Func<TResult> method, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, null, callbackMethod, tag);

        }
        public static void Execute<T, TResult>(Func<T, TResult> method, T arg, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, TResult>(Func<T1, T2, TResult> method, T1 arg1, T2 arg2, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4 }, callbackMethod, tag);
        }
        public static void Execute<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4, arg5 }, callbackMethod, tag);
        }

        public static void Execute<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4, arg5, arg6 }, callbackMethod, tag);
        }

        public static void Execute<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<ThreadProxyCallbackResult> callbackMethod = null, object tag = null)
        {
            new ThreadProxy(method, new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, callbackMethod, tag);
        }
        #endregion  Static Method
    }

    public class ThreadProxyCallbackResult
    {
        public bool HasError { get { return Error != null; } }

        public Exception Error { get; private set; }

        public object Tag { get; private set; }

        private object _result;

        public object Result
        {
            get
            {
                if (HasError)
                {
                    throw Error;
                }
                else
                {
                    return _result;
                }
            }
            private set { _result = value; }
        }


        public ThreadProxyCallbackResult(object tag, Exception e)
        {
            Tag = tag;
            Error = e;
        }

        public ThreadProxyCallbackResult(object tag, object result)
        {
            Tag = tag;
            Result = result;
        }
    }
}
