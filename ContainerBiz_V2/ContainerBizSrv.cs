using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DC;
using BizUtils.Rest;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Threading;
using ConnectorRest_V2;
using DCClient4ICE_Client;
using NLog;

namespace ContainerBiz
{
    internal class ContainerBizSrv
    {
        private IDCClient clt;
        Dictionary<string, IBizSrv> bizs;
        Dictionary<string, IMuteSrv> mutes;
        Logger log = NLog.LogManager.GetCurrentClassLogger();

        internal ContainerBizSrv()
        { }

        internal void Start()
        {
            var serviceId = IniConfig.Read("config", "ServiceId", "*", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ContainerBiz.ini"));

            clt = DCClient.Instance(serviceId);
            bizs = this.getBizSrvs();

            foreach (string url in bizs.Keys)
            {
                bizs[url].SetContext(clt);
                clt.srv(url, bizs[url].RequestProc);
                log.Info("注册接口{0}，来源{1}", url, bizs[url].ToString());
            }

            mutes = this.getMuteSrvs();
            foreach (string url in mutes.Keys)
            {
                mutes[url].Start();
                log.Info("启动服务{0}，来源{1}", url, mutes[url].ToString());
            }
        }

        internal void Stop()
        {
            foreach (string url in mutes.Keys)
            {
                mutes[url].Stop();
            }
        }

        private Dictionary<string, IBizSrv> getBizSrvs()
        {
            Dictionary<string, IBizSrv> result = new Dictionary<string, IBizSrv>();
            //获取项目根目录下的Plugins文件夹
            string dir = Directory.GetCurrentDirectory();
            //遍历目标文件夹中包含dll后缀的文件
            foreach (var file in Directory.GetFiles(dir + @"\", "*.dll"))
            {
                try
                {
                    //加载程序集
                    var asm = Assembly.LoadFrom(file);
                    //遍历程序集中的类型
                    var types = asm.GetTypes().Where((t) => t.BaseType == typeof(IBizSrv));
                    foreach (var type in types)
                    {
                        //创建接口类型实例
                        var ibiz = Activator.CreateInstance(type) as IBizSrv;
                        var sign = "/" + Path.GetFileNameWithoutExtension(file).Replace('.', '/');
                        if (ibiz != null)
                        {
                            result.Add(sign, ibiz);
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error("Recognise {0} failed for:{1}", file, e.Message);
                    continue;
                }
            }
            return result;
        }

        private Dictionary<string, IMuteSrv> getMuteSrvs()
        {
            Dictionary<string, IMuteSrv> result = new Dictionary<string, IMuteSrv>();
            //获取项目根目录下的Plugins文件夹
            string dir = Directory.GetCurrentDirectory();
            //遍历目标文件夹中包含dll后缀的文件
            foreach (var file in Directory.GetFiles(dir + @"\", "*.dll"))
            {
                try
                {
                    //加载程序集
                    var asm = Assembly.LoadFrom(file);
                    //遍历程序集中的类型
                    foreach (var type in asm.GetTypes())
                    {
                        //如果是IMuteSrv接口
                        if (type.GetInterfaces().Contains(typeof(IMuteSrv)))
                        {
                            //创建接口类型实例
                            var ibiz = Activator.CreateInstance(type) as IMuteSrv;
                            var sign = "/" + Path.GetFileNameWithoutExtension(file).Replace('.', '/');
                            if (ibiz != null)
                            {
                                result.Add(sign, ibiz);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error("Recognise {0} failed for:{1}", file, e.Message);
                    continue;
                }
            }
            return result;
        }
    }
}
