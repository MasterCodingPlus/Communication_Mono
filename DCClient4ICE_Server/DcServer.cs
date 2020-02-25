using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dc;
using Ice;
using IceInternal;

namespace DCClient4ICE_Server
{
    public class DcServer : DcClientIceDisp_
    {
        public static LinkedList<DcEntity> dcEntities = new LinkedList<DcEntity>();

        public override byte[] act(string topic, byte[] bytes, Current current = null)
        {
            Console.WriteLine("主题：" + topic + "被调用");

            var urls = topic.Split('?');
            string extInfo = String.Empty;
            if (urls.Length > 1)
            {
                extInfo = urls[1];
            }

            var entity = dcEntities.Where(t => t.topic.StartsWith(urls[0]) && t.eType == DcEntity.EType.SRV);
            if (entity == null)
            {
                throw new System.Exception("主题不存在");
            }

            foreach (var dcEntity in entity)
            {
                var srvCallbackReceiverPrx = (SrvCallbackReceiverPrx) dcEntity.receiver;
                return srvCallbackReceiverPrx.callback(extInfo, bytes);
            }

            throw new System.Exception("主题不存在");
        }

        public override byte[] actDest(string dest, string topic, byte[] bytes, Current current = null)
        {
            Console.WriteLine("主题：" + topic + "被调用，目的" + dest);

            var urls = topic.Split('?');
            string extInfo = String.Empty;
            if (urls.Length > 1)
            {
                extInfo = urls[1];
            }

            var entity = dcEntities.Where(t =>
                t.topic.StartsWith(urls[0]) && t.eType == DcEntity.EType.SRV && dest == t.clientId);
            if (entity == null)
            {
                throw new System.Exception("主题不存在");
            }

            foreach (var dcEntity in entity)
            {
                return ((SrvCallbackReceiverPrx) dcEntity.receiver).callback(extInfo, bytes);
            }

            throw new System.Exception("主题不存在");
        }

        public override Task pubAsync(string topic, byte[] bytes, Current current = null)
        {
            var entity = dcEntities.Where(t => t.topic.StartsWith(topic) && t.eType == DcEntity.EType.SUB);
            if (entity == null)
            {
                return new Task(() => { return; });
            }

            foreach (var dcEntity in entity)
            {
                ((SubCallbackReceiverPrx) dcEntity.receiver).begin_callback(bytes);
            }

            return new Task(() => { return; });
        }

        public override Task pubDestAsync(string dest, string topic, byte[] bytes, Current current = null)
        {
            var entity = dcEntities.Where(t =>
                t.topic.StartsWith(topic) && t.eType == DcEntity.EType.SUB && dest == t.clientId);
            if (entity == null)
            {
                return new Task(() => { return; });
            }

            foreach (var dcEntity in entity)
            {
                ((SubCallbackReceiverPrx) dcEntity.receiver).begin_callback(bytes);
            }

            return new Task(() => { return; });
        }

        public override bool srv(string clientId, string topic, SrvCallbackReceiverPrx receiver, Current current = null)
        {
            var entity = dcEntities.FirstOrDefault(t =>
                t.topic.StartsWith(topic) && t.clientId == clientId && t.eType == DcEntity.EType.SRV);
            if (entity != null)
            {
                dcEntities.Remove(entity);
            }

            DcEntity dcEntity = new DcEntity()
            {
                clientId = clientId,
                topic = topic,
                receiver = receiver
            };
            IPConnectionInfo info = (IPConnectionInfo) current.con.getInfo();
            dcEntity.localAddress = info.localAddress;
            dcEntity.localPort = info.localPort;
            dcEntity.remoteAddress = info.remoteAddress;
            dcEntity.remotePort = info.remotePort;
            dcEntity.createTime = DateTime.Now;
            dcEntity.eType = DcEntity.EType.SRV;
            dcEntities.AddLast(dcEntity);
            Console.WriteLine("收到RPC注册" + dcEntity);
            return true;
        }

        public override bool sub(string clientId, string topic, SubCallbackReceiverPrx receiver, Current current = null)
        {
            var entity = dcEntities.FirstOrDefault(t =>
                t.topic.StartsWith(topic) && t.clientId == clientId && t.eType == DcEntity.EType.SUB);
            if (entity != null)
            {
                dcEntities.Remove(entity);
            }

            DcEntity dcEntity = new DcEntity()
            {
                clientId = clientId,
                topic = topic,
                receiver = receiver
            };

            IPConnectionInfo info = (IPConnectionInfo) current.con.getInfo();
            dcEntity.localAddress = info.localAddress;
            dcEntity.localPort = info.localPort;
            dcEntity.remoteAddress = info.remoteAddress;
            dcEntity.remotePort = info.remotePort;
            dcEntity.createTime = DateTime.Now;
            dcEntity.eType = DcEntity.EType.SRV;
            dcEntities.AddLast(dcEntity);
            Console.WriteLine("收到sub注册" + dcEntity);
            return true;
        }
    }
}