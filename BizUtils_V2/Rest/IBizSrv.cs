using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectorRest_V2;
using DCClient4ICE_Client;
using NLog;

namespace BizUtils.Rest
{
    public abstract class IBizSrv
    {
        protected Logger log = NLog.LogManager.GetCurrentClassLogger();
        public abstract byte[] RequestProc(string extInfo, byte[] req);
        public abstract void SetContext(IDCClient dcClient);

        protected string readConfig(string key)
        {
            return IniConfig.Read("config", key, "*", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ContainerBiz.ini"));
        }
    }

    public interface IMuteSrv
    {
        void Start();
        void Stop();
    }
}