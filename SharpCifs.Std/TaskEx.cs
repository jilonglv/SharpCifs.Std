namespace SharpCifs.Std
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    public static class TaskEx
    {
        public static Task TaskRun(Action action)
        {
#if NET40
            return Run(action);
#else
            return System.Threading.Tasks.Task.Run(action);
#endif
        }
        public static Task<TResult> TaskRun<TResult>(Func<TResult> action)
        {
#if NET40
            return Run(action);
#else
            return System.Threading.Tasks.Task.Run(action);
#endif
        }
        public static Task TaskRun(Action action, CancellationToken token)
        {
#if NET40
            return Run(action, token);
#else
            return System.Threading.Tasks.Task.Run(action, token);
#endif
        }
        public static Task Run(Action action, CancellationToken token)
        {
            TaskFactory factory = new TaskFactory(token);
            return factory.StartNew(action, token);
        }
        public static Task Run(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            new Thread(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            })
            { IsBackground = true }.Start();
            return tcs.Task;
        }
        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            var tcs = new TaskCompletionSource<TResult>();
            new Thread(() =>
            {
                try
                {
                    tcs.SetResult(function());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            })
            { IsBackground = true }.Start();
            return tcs.Task;
        }

        public static Task Delay(int milliseconds)
        {
#if NET40
            return TaskEx.Run(() => Thread.Sleep(milliseconds));
#else
            return Task.Delay(milliseconds);
#endif
        }
    }
}

