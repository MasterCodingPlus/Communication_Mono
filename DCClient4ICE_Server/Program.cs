using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DCClient4ICE_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://0.0.0.0:7788/");
            //使用新的方式
            WebServiceHost _serviceHost = new WebServiceHost(new DcEntityQuery(), baseAddress);
            //如果不设置MaxBufferSize,当传输的数据特别大的时候，很容易出现“提示:413 Request Entity Too Large”错误信息,最大设置为20M
            WebHttpBinding binding = new WebHttpBinding
            {
                TransferMode = TransferMode.Buffered,
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                MaxBufferPoolSize = 2147483647,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                Security = { Mode = WebHttpSecurityMode.None }
            };
            _serviceHost.AddServiceEndpoint(typeof(IDcEntityQuery), binding, baseAddress);
            _serviceHost.Opened += delegate { Console.WriteLine("Web服务已开启..."); };
            _serviceHost.Open();
            using (var communicator = Ice.Util.initialize(ref args, "config.server"))
            {
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    eventArgs.Cancel = true;
                    communicator.shutdown();
                };

                var adapter = communicator.createObjectAdapter("dcServer");
                adapter.add(new DcServer(), Ice.Util.stringToIdentity("dcServer"));

                try
                {
                    adapter.activate();
                    communicator.waitForShutdown();
                    _serviceHost.Close();
                }
                finally
                {
                }
            }
        }
    }
}