using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCClient4ICE_Client
{
    public interface IDCClient : IDisposable
    {
        /// <summary>
        /// 将数据块以指定类型发出。
        /// </summary>
        /// <param name="type">指定的数据类型</param>
        /// <param name="data">待发送的数据块</param>
        void pub(string type, byte[] data);

        void pub(string type, byte[] data, int timeoutSeconds);

        void pubAsync(string type, byte[] data, Action<byte[]> callback);

        /// <summary>
        /// 订阅指定类型数据，以指定的委托处理到达的数据块。可指定多个处理委托
        /// </summary>
        /// <param name="type">指定订阅的数据类型</param>
        /// <param name="callback">指定数据块消费者（处理委托）</param>
        void sub(string type, Action<byte[]> callback);

        /// <summary>
        /// 订阅指定发送方发出的指定类型数据，以指定的委托处理到达的数据块。可指定多个处理委托
        /// </summary>
        /// <param name="type">指定订阅的数据类型</param>
        /// <param name="publisher">指定数据发送方</param>
        /// <param name="callback">指定数据块消费者（处理委托）</param>
        void sub(string type, string publisher, Action<byte[]> callback);

        /// <summary>
        /// 取消数据订阅
        /// </summary>
        /// <param name="type">取消订阅的数据类型</param>
        /// <param name="callback">取消的数据块消费者（处理委托）</param>
        void unSub(string type, Action<byte[]> callback);

        /// <summary>
        /// 取消数据订阅
        /// </summary>
        /// <param name="type">取消订阅的数据类型</param>
        /// <param name="publisher">指定数据发送方</param>
        /// <param name="callback">指定数据块消费者（处理委托）</param>
        void unSub(string type, string publisher, Action<byte[]> callback);

        /// <summary>
        /// 拉取一条数据
        /// </summary>
        /// <param name="type">指定请求的数据类型</param>
        /// <returns>数据最新快照，无数据时返回NULL</returns>
        byte[] take(string type);

        /// <summary>
        /// 拉取指定数据类型的最新快照
        /// </summary>
        /// <param name="type">指定请求的数据类型</param>
        /// <param name="publisher">指定请求的数据发送方</param>
        /// <returns>数据最新快照，无数据时返回NULL</returns>
        byte[] take(string type, string publisher);

        /// <summary>
        /// 向指定目的方发出指定类型的数据块处理请求。默认超时时长
        /// </summary>
        /// <param name="type">指定数据块处理请求类型</param>
        /// <param name="dest">指定目的方</param>
        /// <param name="data">待处理的数据块</param>
        /// <returns>数据块处理结果</returns>
        ActResult act(string type, string dest, byte[] data);

        ActResult act(string type, string dest, byte[] data, int timeoutSeconds);

        void actAsync(string type, string dest, byte[] data, Action<ActResult> callback);

        /// <summary>
        /// 向不确定目的方发出指定类型的数据块处理请求，默认超时时长
        /// </summary>
        /// <param name="type">指定数据块处理请求类型</param>
        /// <param name="data">待处理的数据块</param>
        /// <returns>数据块处理结果</returns>
        ActResult act(string type, byte[] data);

        ActResult act(string type, byte[] data, int timeoutSeconds);

        void actAsync(string type, byte[] data, Action<ActResult> callback);

        /// <summary>
        /// 注册处理指定类型的数据块。每个DcClient实例每个类型只能注册一个处理委托
        /// </summary>
        /// <param name="type">注册处理的数据块类型</param>
        /// <param name="callback">指定到达的数据块的处理委托</param>
        void srv(string type, Func<string, byte[], byte[]> callback);

        /// <summary>
        /// 取消注册处理指定类型的数据块，每个DcClient7实例每个类型只能注册一个处理委托
        /// </summary>
        /// <param name="type">注册处理的数据块类型</param>
        /// <param name="callback">指定到达的数据块的处理委托</param>
        void unSrv(string type, Func<string, byte[], byte[]> callback);

        /// <summary>
        /// 向指定目的方发出指定类型的数据块
        /// </summary>
        /// <param name="type">注册处理的数据块类型</param>
        /// <param name="dest">指定目的方</param>
        /// <param name="data">待发送的数据块</param>
        void snd(string type, string dest, byte[] data);

        void snd(string type, string dest, byte[] data, int timeoutSeconds);

        void sndAsync(string type, string dest, byte[] data, Action<byte[]> callback);

        /// <summary>
        /// 向不确定目的方发出指定类型的数据块
        /// </summary>
        /// <param name="type">注册处理的数据块类型</param>
        /// <param name="data">待发送的数据块</param>
        void snd(string type, byte[] data);

        void snd(string type, byte[] data, int timeoutSeconds);

        void sndAsync(string type, byte[] data, Action<byte[]> callback);

        /// <summary>
        /// 注册接收指定类型的数据块
        /// </summary>
        /// <param name="type">注册处理的数据块类型</param>
        /// <param name="callback">指定到达的数据块的处理委托</param>
        void rcv(string type, Action<byte[]> callback);

        /// <summary>
        /// 取消注册接收指定类型的数据块
        /// </summary>
        /// <param name="type">注册处理的数据块类型</param>
        /// <param name="callback">数据块的处理委托</param>
        void unRcv(string type, Action<byte[]> callback);

    }
}