using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using DCClient4ICE_Server;

namespace RestfulService
{
    // 定义两种方法，1、GetInputFunc：input，返回input；2、PostInputFunc：通过POST请求，传入input，返回input
    [ServiceContract(Name = "GetInputService")]
    public interface IRestFulDemoServices
    {
        //说明：GET请求
        //WebGet默认请求是GET方式
        //UriTemplate(URL Routing)input必须要方法的参数名必须一致不区分大小写）
        [OperationContract]
        [WebGet(UriTemplate = "GetInput/{input}", BodyStyle = WebMessageBodyStyle.Bare)]
        string GetInputFunc(string input);

        //说明：POST请求
        //WebInvoke请求方式有POST、PUT、DELETE等，所以需要明确指定Method是哪种请求的，这里我们设置POST请求。
        //UriTemplate(URL Routing)input必须要方法的参数名必须一致不区分大小写）
        //RequestFormat规定客户端必须是什么数据格式请求的（JSon或者XML），不设置默认为XML
        // ResponseFormat规定服务端返回给客户端是以是什么数据格返回的（JSon或者XML）
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetInput/{input}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DcEntity PostInputFunc(string input);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestFulDemoServices : IRestFulDemoServices
    {
        public string GetInputFunc(string Input)
        {
            return Input;
        }
        public DcEntity PostInputFunc(string Input)
        {
            return new DcEntity() {clientId = "456"};
        }
    }
    [DataContract]
    public class User
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public int Score { get; set; }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            RestFulDemoServices demoServices = new RestFulDemoServices();
    //            WebServiceHost _serviceHost = new WebServiceHost(demoServices, new Uri("http://localhost:8000/"));
    //            _serviceHost.Open();
    //            Console.WriteLine("Web服务已开启...");
    //            Console.WriteLine("输入任意键关闭程序！");
    //            Console.ReadKey();
    //            _serviceHost.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Web服务开启失败：{0}\r\n{1}", ex.Message, ex.StackTrace);
    //        }
    //    }
    //}
}
