using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using RestSharp;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using BizUtils.Rest;
using ConnectorRest_V2;
using DCClient4ICE_Client;
using NLog;

namespace ConnectorRest
{
    public class ConnRestService
    {
        HttpListener listener = new HttpListener();
        static Logger log = NLog.LogManager.GetCurrentClassLogger();

        void GetContextCallBack(IAsyncResult ar)
        {
            HttpListener listener = ar.AsyncState as HttpListener;
            listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
            try
            {
                HttpListenerContext context = listener.EndGetContext(ar);
                ThreadPool.QueueUserWorkItem(o =>
                {
                    HttpListenerContext c = o as HttpListenerContext;
                    procContext(c);
                }, context);
            }
            catch (Exception ex)
            {
                log.Error("listener.BeginGetContext error:{0}", ex.Message);
            }
        }

        private void procContext(HttpListenerContext context)
        {
            // 取得请求对象
            HttpListenerRequest request = context.Request;
            // 取得回应对象
            HttpListenerResponse response = context.Response;

            try
            {
                byte[] postData = parseRequest(request);
                var dcClient = DC.DCClient.Instance();
                ;
                var actResult = dcClient.act(HttpUtility.UrlDecode(request.RawUrl), postData);
                issueResponse(request, response, actResult);
            }
            catch (Exception ex)
            {
                // 设置回应头部内容，长度，编码
                byte[] respbytes = Encoding.UTF8.GetBytes(ex.Message);
                response.ContentLength64 = respbytes.LongLength;
                response.ContentType = "text/plain; charset=UTF-8";
                response.StatusCode = 500;
                // 输出回应内容
                try
                {
                    if (ex is MissingMethodException)
                    {
                        log.Warn(ex.Message);
                    }
                    else
                    {
                        log.Error(ex.Message);
                    }

                    using (BinaryWriter writer = new BinaryWriter(response.OutputStream))
                    {
                        writer.Write(respbytes, 0, (int) response.ContentLength64);
                    }
                }
                catch (Exception ex2)
                {
                    log.Error(ex2.Message);
                }

                //response.Close();
            }
        }

        static int logTxtLen = 2048;

        private static void issueResponse(HttpListenerRequest request, HttpListenerResponse response, ActResult rst)
        {
            if (rst.Exception != null)
            {
                rst.ResultData = PackOrb.PackRespose(
                    new HttpHeadInfo {StatusCode = BizUtils.Rest.HttpStatusCode.ServerSideError},
                    new ListDictionary
                    {
                        {"respnum", -1},
                        {"respmsg", rst.Exception},
                    }
                );

                log.Error("issueResponse get a biz srv error:{0}", rst.Exception.Message);
            }

            // 设置回应头部内容，长度，编码
            int httpHeadInfoLength = BitConverter.ToInt32(rst.ResultData, 0);
            HttpHeadInfo httpHeadInfo = HttpHeadInfo.FromBytes(rst.ResultData, 4, httpHeadInfoLength);
            int rawBytesIndex = httpHeadInfoLength + 4;
            response.ContentLength64 = rst.ResultData.LongLength - rawBytesIndex;
            response.ContentType = httpHeadInfo.ContentType;
            response.StatusCode = (int) httpHeadInfo.StatusCode;
            response.AddHeader("Access-Control-Allow-Methods", "*");

            string url = request.Url.ToString();
            //var format = url.Split(new char [] {'='});
            //var picFormat = format[1].Split(new char[] { '.' });

            if ((url.EndsWith(".jpg") || url.EndsWith(".png")) && Regex.IsMatch(url, "readfile"))
            {
                response.AddHeader("Content-Type", "image/jpeg");
            }
            else if (url.EndsWith(".zip") && Regex.IsMatch(url, "readfile"))
            {
                response.AddHeader("Content-Type", "application/x-zip-compressed");
            }
            else
            {
                response.AddHeader("Content-Type", "application/json;charset=utf-8");
            }

            response.AddHeader("Access-Control-Allow-Origin", "*");
            //var urlstr = url.Split(new char[] { '=' });
            //var filename = HttpUtility.UrlEncode(urlstr[1], Encoding.UTF8);
            //response.AddHeader("Content-Disposition", "attachment; filename="+ filename);//加这个就成下载文件了，不加就成显示了
            // response.StatusDescription = httpHeadInfo.StatusDescription;
            // 输出回应内容
            try
            {
                using (BinaryWriter writer = new BinaryWriter(response.OutputStream))
                {
                    writer.Write(rst.ResultData, rawBytesIndex, (int) response.ContentLength64);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            if (response.ContentType.Equals(HttpContentType.Json))
            {
                if (response.ContentLength64 > logTxtLen)
                {
                    if (response.StatusCode == (int) BizUtils.Rest.HttpStatusCode.Succeed)
                    {
                        log.Trace(string.Format(
                                "{0}<--{1}:::{2}",
                                request.RemoteEndPoint,
                                request.RawUrl,
                                Encoding.UTF8.GetString(rst.ResultData, rawBytesIndex, logTxtLen) + "..."
                            )
                        );
                    }
                    else
                    {
                        log.Warn(string.Format(
                                "{0}<--{1}:::{2}",
                                request.RemoteEndPoint,
                                request.RawUrl,
                                Encoding.UTF8.GetString(rst.ResultData, rawBytesIndex, logTxtLen) + "..."
                            )
                        );
                    }
                }
                else
                {
                    if (response.StatusCode == (int) BizUtils.Rest.HttpStatusCode.Succeed)
                    {
                        log.Trace(string.Format(
                                "{0}<--{1}:::{2}",
                                request.RemoteEndPoint,
                                request.RawUrl,
                                Encoding.UTF8.GetString(rst.ResultData, rawBytesIndex, (int) response.ContentLength64)
                            )
                        );
                    }
                    else
                    {
                        log.Warn(string.Format(
                                "{0}<--{1}:::{2}",
                                request.RemoteEndPoint,
                                request.RawUrl,
                                Encoding.UTF8.GetString(rst.ResultData, rawBytesIndex, (int) response.ContentLength64)
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// 转换为ISO_8859_1
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        private string StringToISO_8859_1(string srcText)
        {
            string dst = "";
            char[] src = srcText.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                string str = @"&#" + (int) src[i] + ";";
                dst += str;
            }

            return dst;
        }

        private byte[] parseRequest(HttpListenerRequest request)
        {
            int len = (int) request.ContentLength64;
            if (request.HttpMethod == "POST" && (!request.HasEntityBody || len <= 0))
            {
                log.Trace(string.Format(
                        "{0}-->{1}:::{2}",
                        request.RemoteEndPoint,
                        request.RawUrl,
                        "无POST BODY的请求."
                    )
                );
                return new byte[0];
            }
            else if (request.HttpMethod == "GET")
            {
                NameValueCollection collection = request.QueryString;
                ListDictionary param = new ListDictionary();
                foreach (string item in collection.AllKeys)
                {
                    param.Add(item, collection.Get(item));
                }

                byte[] _buffer = System.Text.Encoding.UTF8.GetBytes(PackOrb.ObjToJson(param));
                return _buffer;
            }

            byte[] buffer;

            using (BinaryReader reader = new BinaryReader(request.InputStream))
            {
                buffer = reader.ReadBytes(len);
            }

            if (request.ContentLength64 > 0 && request.ContentLength64 <= 1024)
            {
                log.Trace(string.Format(
                        "{0}-->{1}:::{2}",
                        request.RemoteEndPoint,
                        request.RawUrl,
                        Encoding.UTF8.GetString(buffer)
                    )
                );
            }
            else if (request.ContentLength64 > 1024 && request.ContentLength64 <= 4096)
            {
                log.Trace(string.Format(
                        "{0}-->{1}:::{2}",
                        request.RemoteEndPoint,
                        request.RawUrl,
                        Encoding.UTF8.GetString(buffer, 0, 1024) + "..."
                    )
                );
            }
            else
            {
                log.Trace(string.Format(
                        "{0}-->{1}:::{2}",
                        request.RemoteEndPoint,
                        request.RawUrl,
                        string.Format("{0} bytes long content.", request.ContentLength64)
                    )
                );
            }

            return buffer;
        }

        public void Start()
        {
            var config = IniConfig.Read("init", "ListenPoint", "http://+:8080/,https://+:8081/", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectorRest.ini"));
            log.Info("开始启动服务:{0}", config);

            // 检查系统是否支持
            if (!HttpListener.IsSupported)
            {
                log.Error("软件运行需要 Windows XP SP2 或 Server 2003 以上系统.");
            }

            // 注意前缀必须以 / 正斜杠结尾
            string[] prefixes = config.Split(',');

            // 设置manifest UAC权限
            foreach (string lsnrUrl in prefixes)
            {
                AddAddress(lsnrUrl);
            }

            // 增加监听的前缀.
            foreach (string lsnrUrl in prefixes)
            {
                listener.Prefixes.Add(lsnrUrl);
            }

            //开始监听
            try
            {
                listener.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }

            foreach (string lsnrUrl in prefixes)
            {
                log.Info(string.Format("REST监听启动于：{0}", lsnrUrl));
            }


            //for (int i = 0; i < 60; i++)
            {
                try
                {
                    listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
                }
                catch (Exception ex)
                {
                    log.Error("listener.BeginGetContext error:{0}", ex.Message);
                }
            }

            Console.Read();
        }

        public void Stop()
        {
            try
            {
                //listener.Stop();
                listener.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }


        void AddAddress(string address)
        {
            try
            {
                AddAddress(address, Environment.UserDomainName, Environment.UserName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        void AddAddress(string address, string domain, string user)
        {
            string argsDll = String.Format(@"http delete urlacl url={0}", address);
            string args = string.Format(@"http add urlacl url={0} user={1}\{2}", address, domain, user);
            ProcessStartInfo psi = new ProcessStartInfo("netsh", argsDll);
            psi.Verb = "runas";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            Process.Start(psi).WaitForExit(); //删除urlacl
            psi = new ProcessStartInfo("netsh", args);
            psi.Verb = "runas";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            Process.Start(psi).WaitForExit(); //添加urlacl
        }
    }
}