using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.LogHelper
{
    internal class _LogHelper
    {
        private ConcurrentQueue<LogItem> queue = new ConcurrentQueue<LogItem>();
        private object _lock = new object();
        private Task task;

        private readonly static Lazy<_LogHelper> _instance = new Lazy<_LogHelper>(() => new _LogHelper());

        internal static _LogHelper Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void Log(string message, string detail = "")
        {
            LogItem item = new LogItem(message, detail);
            queue.Enqueue(item);

            if (task == null || task.IsCompleted)
            {
                CreateTask();
            }
        }

        private void Write(string message, string detail)
        {
            string logPath = "abc.log";
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    sw.WriteLine(message);
                }
                if (!string.IsNullOrEmpty(detail))
                {
                    sw.WriteLine(detail);
                }
            }
        }

        private void CreateTask()
        {
            lock (_lock)
            {
                if (task == null || task.IsCompleted)
                {
                    task = Task.Run(() => Run());
                }
            }
        }

        private void Run()
        {
            LogItem log = null;
            while (queue.TryDequeue(out log))
            {
                Write(log.Message, log.Detail);
            }
        }
    }
}
