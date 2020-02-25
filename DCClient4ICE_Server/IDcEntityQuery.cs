using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DCClient4ICE_Server
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract(Name = "IDcEntityQuery")]
    public interface IDcEntityQuery
    {
        /// <summary>
        /// 说明：POS请求
        /// WebInvoke请求方式有POST、PUT、DELETE等，所以需要明确指定Method是哪种请求的，这里我们设置POST请求。
        /// 注意：POST情况下，UriTemplate(URL Routing)一般是没有参数（和上面GET的UriTemplate不一样，因为POST参数都通过消息体传送）
        /// RequestFormat规定客户端必须是什么数据格式请求的（JSon或者XML），不设置默认为XML
        /// ResponseFormat规定服务端返回给客户端是以是什么数据格返回的（JSon或者XML）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "allInfo", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<DcEntity> GetInfo();

        /// <summary>
        /// 说明：POS请求
        /// WebInvoke请求方式有POST、PUT、DELETE等，所以需要明确指定Method是哪种请求的，这里我们设置POST请求。
        /// 注意：POST情况下，UriTemplate(URL Routing)一般是没有参数（和上面GET的UriTemplate不一样，因为POST参数都通过消息体传送）
        /// RequestFormat规定客户端必须是什么数据格式请求的（JSon或者XML），不设置默认为XML
        /// ResponseFormat规定服务端返回给客户端是以是什么数据格返回的（JSon或者XML）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "allInfo", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<DcEntity> GetInfoGET();
    }
}