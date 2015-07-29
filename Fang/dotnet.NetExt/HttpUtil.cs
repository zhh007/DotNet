using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace dotnet.NetExt
{
    public class HttpUtil
    {
        private readonly string UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private string requestEncoding = "utf-8";
        private string responseEncoding = "utf-8";
        private CookieContainer cookieContainer = new CookieContainer();

        public HttpUtil(string reqEncoding, string respEncoding)
        {
            requestEncoding = reqEncoding;
            responseEncoding = respEncoding;
        }

        public string Get(string url)
        {
            string result = "";

            try
            {
                HttpWebRequest request = CreateRequest(url);
                request.Method = "GET";
                request.UserAgent = UserAgent;
                request.Referer = GetDomainUrl(url);
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.GetEncoding(responseEncoding)))
                        {
                            result = readStream.ReadToEnd();
                        }
                    }
                }

                DebugCookies(cookieContainer.GetCookies(request.RequestUri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public string Post(string url, IDictionary<string, string> parameters)
        {
            string result = "";
            byte[] data = new byte[0];
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, HttpUtility.UrlPathEncode(parameters[key]));
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, HttpUtility.UrlPathEncode(parameters[key]));
                    }
                    i++;
                }
                Encoding encoding = Encoding.GetEncoding(requestEncoding);
                data = encoding.GetBytes(buffer.ToString());
            }

            HttpWebRequest httpRequest = CreateRequest(url);
            httpRequest.UserAgent = UserAgent;
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.Method = "POST";
            httpRequest.Referer = GetDomainUrl(url);
            httpRequest.CookieContainer = cookieContainer;

            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
            {
                //Console.WriteLine(response.Headers["Set-Cookie"]);
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, Encoding.GetEncoding(responseEncoding)))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }

            DebugCookies(cookieContainer.GetCookies(httpRequest.RequestUri));

            return result;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        private HttpWebRequest CreateRequest(string url)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }

            return request;
        }

        private string GetDomainUrl(string url)
        {
            Uri uri = new Uri(url);
            return uri.Scheme + "://" + uri.Authority + "/";
        }

        private void DebugCookies(CookieCollection cookies)
        {
            if (cookies != null)
            {
                var enumerator = cookies.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Cookie c = enumerator.Current as Cookie;
                    if (c != null)
                    {
                        Console.WriteLine("{0}={1}", c.Name, c.Value);
                    }
                }
            }
        }

    }
}
