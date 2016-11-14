using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    public class ConcurrentRunner : IDisposable
    {
        #region 私有方法 
        /// <summary> 
        /// 测试方法所在的接口 
        /// </summary> 
        private Func<bool> func;
        /// <summary> 
        /// 主线程控制信号 
        /// </summary> 
        private ManualResetEvent manualResetEvent;
        /// <summary> 
        /// 测试线程控制信号 
        /// </summary> 
        private ManualResetEvent threadResetEvent;
        /// <summary> 
        /// 待执行的线程数 
        /// </summary> 
        private List<int> threads;
        /// <summary> 
        /// 测试结果 
        /// </summary> 
        private List<ConcurrentRunItemResult> results;
        /// <summary> 
        /// 执行测试的成功数 
        /// </summary> 
        private int successCount;
        /// <summary> 
        /// 执行测试的失败数 
        /// </summary> 
        private int failureCount;
        /// <summary> 
        /// 测试耗时 
        /// </summary> 
        private long elapsedMilliseconds;
        /// <summary> 
        /// 当前线程 
        /// </summary> 
        private int currentIndex;
        /// <summary> 
        /// 当前测试的总线程数 
        /// </summary> 
        private int currentCount;
        /// <summary> 
        /// 思考时间 
        /// </summary> 
        private int thinkTime;
        /// <summary> 
        /// 重复次数 
        /// </summary> 
        private int repeatCount;
        /// <summary> 
        /// 测试计时器 
        /// </summary> 
        private Stopwatch stopwatch;
        #endregion

        #region 构造函数 
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        public ConcurrentRunner()
        {
            manualResetEvent = new ManualResetEvent(true);
            threadResetEvent = new ManualResetEvent(true);
            stopwatch = new Stopwatch();
        }
        #endregion

        #region 执行测试 
        /// <summary> 
        /// 执行多线程测试 
        /// </summary> 
        /// <param name="threadCount">需要测试的线程数</param> 
        /// <param name="func">待执行方法</param> 
        /// <returns></returns> 
        public List<ConcurrentRunItemResult> Execute(int threadCount, Func<bool> func)
        {
            return Execute(threadCount, 1, func);
        }
        /// <summary> 
        /// 执行多线程测试 
        /// </summary> 
        /// <param name="threadCount">需要测试的线程数</param> 
        /// <param name="repeatCount">重复次数</param> 
        /// <param name="func">待执行方法</param> 
        /// <returns></returns> 
        public List<ConcurrentRunItemResult> Execute(int threadCount, int repeatCount, Func<bool> func)
        {
            return Execute(threadCount, 0, repeatCount, func);
        }
        /// <summary> 
        /// 执行多线程测试 
        /// </summary> 
        /// <param name="threadCount">需要测试的线程数</param> 
        /// <param name="thinkTime">思考时间，单位耗秒</param> 
        /// <param name="repeatCount">重复次数</param> 
        /// <param name="func">待执行方法</param> 
        /// <returns></returns> 
        public List<ConcurrentRunItemResult> Execute(int threadCount, int thinkTime, int repeatCount, Func<bool> func)
        {
            return Execute(new List<int>() { threadCount }, thinkTime, repeatCount, func);
        }
        /// <summary> 
        /// 执行多线程测试 
        /// </summary> 
        /// <param name="threads">分别需要测试的线程数</param> 
        /// <param name="thinkTime">思考时间，单位耗秒</param> 
        /// <param name="repeatCount">重复次数</param> 
        /// <param name="func">待执行方法</param> 
        /// <returns></returns> 
        public List<ConcurrentRunItemResult> Execute(List<int> threads, int thinkTime, int repeatCount, Func<bool> func)
        {
            this.func = func;
            this.threads = threads;
            this.thinkTime = thinkTime;
            this.repeatCount = repeatCount;
            CheckParameters();
            CreateMultiThread();
            return this.results;
        }
        #endregion

        #region 验证参数 
        /// <summary> 
        /// 验证参数 
        /// </summary> 
        private void CheckParameters()
        {
            if (func == null) throw new ArgumentNullException("func不能为空");
            if (threads == null || threads.Count == 0) throw new ArgumentNullException("threads不能为空或者长度不能为0");
            if (thinkTime < 0) throw new Exception("thinkTime不能小于0");
            if (repeatCount <= 0) throw new Exception("repeatCount不能小于等于0");
        }
        #endregion

        #region 创建多线程并执行测试 
        /// <summary> 
        /// 创建多线程进行测试 
        /// </summary> 
        private void CreateMultiThread()
        {
            results = new List<ConcurrentRunItemResult>(threads.Count);
            foreach (int threadCount in threads)
            {
                for (int repeat = 0; repeat < repeatCount; repeat++)
                {
                    //主线程进入阻止状态 
                    manualResetEvent.Reset();
                    //测试线程进入阻止状态 
                    threadResetEvent.Reset();
                    stopwatch.Reset();
                    currentCount = threadCount;
                    currentIndex = 0;
                    successCount = 0;
                    failureCount = 0;
                    elapsedMilliseconds = 0;
                    for (int i = 0; i < currentCount; i++)
                    {
                        Thread t = new Thread(new ThreadStart(DoWork));
                        t.Start();
                    }
                    //阻止主线程，等待测试线程完成测试 
                    manualResetEvent.WaitOne();
                    results.Add(new ConcurrentRunItemResult()
                    {
                        FailureCount = failureCount,
                        SuccessCount = successCount,
                        ElapsedMilliseconds = elapsedMilliseconds
                    });
                    Thread.Sleep(thinkTime);
                }
            }
        }
        /// <summary> 
        /// 执行测试方法 
        /// </summary> 
        private void DoWork()
        {
            bool executeResult;
            Interlocked.Increment(ref currentIndex);
            if (currentIndex < currentCount)
            {
                //等待所有线程创建完毕后同时执行测试 
                threadResetEvent.WaitOne();
            }
            else
            {
                //最后一个线程创建完成，通知所有线程，开始执行测试 
                threadResetEvent.Set();
                //开始计时 
                stopwatch.Start();
            }
            //执行测试 
            executeResult = func();
            Interlocked.Decrement(ref currentIndex);
            if (currentIndex == 0)
            {
                //最后一个线程执行的测试结束，结束计时 
                stopwatch.Stop();
                elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                //保存测试结果 
                if (executeResult)
                    Interlocked.Increment(ref successCount);
                else
                    Interlocked.Increment(ref failureCount);
                //通知主线程继续 
                manualResetEvent.Set();
            }
            else
            {
                //保存测试结果 
                if (executeResult)
                    Interlocked.Increment(ref successCount);
                else
                    Interlocked.Increment(ref failureCount);
            }
        }
        #endregion

        #region 释放资源 
        /// <summary> 
        /// 释放资源 
        /// </summary> 
        public void Dispose()
        {
            manualResetEvent.Close();
            threadResetEvent.Close();
        }
        #endregion
    }
    /// <summary> 
    /// 并发测试结果 
    /// </summary> 
    public class ConcurrentRunItemResult
    {
        /// <summary> 
        /// 当前执行线程总数 
        /// </summary> 
        public int ThreadCount
        {
            get { return SuccessCount + FailureCount; }
        }
        /// <summary> 
        /// 测试成功数 
        /// </summary> 
        public int SuccessCount { get; set; }
        /// <summary> 
        /// 测试失败数 
        /// </summary> 
        public int FailureCount { get; set; }
        /// <summary> 
        /// 总耗时 
        /// </summary> 
        public long ElapsedMilliseconds { get; set; }
    }
}
