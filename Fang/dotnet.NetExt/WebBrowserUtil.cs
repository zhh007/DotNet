﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotnet.NetExt
{
    public class WebBrowserUtil
    {
        private string requestEncoding = "utf-8";
        private string responseEncoding = "utf-8";

        public WebBrowserUtil(string reqEncoding, string respEncoding)
        {
            requestEncoding = reqEncoding;
            responseEncoding = respEncoding;
        }

        //public void Get(string url)
        //{
        //    string[] arr = url.Split(new char[] { '/' });
        //    filename = arr[arr.Length - 1];

        //    Thread thread = new Thread(new ParameterizedThreadStart(BeginCatch));
        //    thread.SetApartmentState(ApartmentState.STA);
        //    thread.Start(url);
        //}

        //void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{

        //    WebBrowser web = (WebBrowser)sender;

        //    if (web.ReadyState != WebBrowserReadyState.Complete)
        //        return;

        //    System.IO.StreamReader getReader = new System.IO.StreamReader(web.DocumentStream, System.Text.Encoding.GetEncoding("gbk"));
        //    string txt = getReader.ReadToEnd();

        //    //string txt = web.DocumentText;
        //    string fpath = System.IO.Path.Combine(Environment.CurrentDirectory, "DetailPage");
        //    fpath = System.IO.Path.Combine(fpath, filename);
        //    File.WriteAllText(fpath, txt, Encoding.GetEncoding("gbk"));

        //    Application.ExitThread();
        //}

        //private void BeginCatch(object obj)
        //{
        //    string url = obj.ToString();
        //    WebBrowser wb = new WebBrowser();

        //    wb.Navigate(url);
        //    wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);
        //    Application.Run();
        //}

        public void DownloadPage(string dir, string[] urls, string[] filenames)
        {
            try
            {
                var task = MessageLoopWorker.Run(DoWorkAsync, dir, urls, filenames);
                task.Wait();
                Console.WriteLine("DoWorkAsync completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DoWorkAsync failed: " + ex.Message);
            }
        }

        private async Task<object> DoWorkAsync(params object[] args)
        {
            Console.WriteLine("Start working.");

            string dir = args[0] as string;
            string[] urls = args[1] as string[];
            string[] filenames = args[2] as string[];
            Random rnd = new Random();

            using (var wb = new WebBrowser())
            {
                wb.ScriptErrorsSuppressed = true;

                TaskCompletionSource<bool> tcs = null;
                WebBrowserDocumentCompletedEventHandler documentCompletedHandler = (s, e) =>
                {
                    WebBrowser web = (WebBrowser)s;
                    if (web.ReadyState != WebBrowserReadyState.Complete)
                        return;
                    tcs.TrySetResult(true);
                };

                int len = urls.Length;
                for (int i = 0; i < urls.Length; i++)
                {
                    Thread.Sleep(rnd.Next(0, 30) * 10);//0~300
                    string url = urls[i];
                    string filename = filenames[i];
                    string fpath = System.IO.Path.Combine(dir, filename);

                    Console.WriteLine("{0}/{1} {2}", i + 1, len, url);

                    if (File.Exists(fpath))
                    {
                        Console.WriteLine("已存在。");
                        continue;
                    }

                    tcs = new TaskCompletionSource<bool>();
                    wb.DocumentCompleted += documentCompletedHandler;
                    try
                    {
                        wb.Navigate(url);
                        // await for DocumentCompleted
                        await tcs.Task;
                    }
                    finally
                    {
                        wb.DocumentCompleted -= documentCompletedHandler;
                    }
                    
                    // the DOM is ready
                    using (var stream = wb.DocumentStream)
                    {
                        using (StreamReader getReader = new StreamReader(stream, Encoding.GetEncoding(responseEncoding)))
                        {
                            string txt = getReader.ReadToEnd();
                            //File.WriteAllText(fpath, txt, Encoding.GetEncoding("gbk"));
                            File.WriteAllText(fpath, txt);

                            //CsQuery.CQ dom = txt;//, Encoding.GetEncoding("gbk")
                            //string title = dom["title"].Text();
                            //Console.WriteLine(title);
                        }
                    }
                }
            }

            Console.WriteLine("End working.");
            return null;
        }
    }
}
