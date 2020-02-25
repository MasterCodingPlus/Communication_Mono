using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ice;

namespace DCClient4ICE_Server
{
    [DataContract]
    public class DcEntity
    {
        [DataMember]
        public string clientId;
        [DataMember]
        public string topic;
        public ObjectPrx receiver;
        [DataMember]
        public string localAddress;
        [DataMember]
        public int localPort;
        [DataMember]
        public string remoteAddress;
        [DataMember]
        public int remotePort;
        public DateTime createTime;
        [DataMember]
        public EType eType;

        public override string ToString()
        {
            return "DcEntity{" +
                   "eType='" + eType + '\'' +
                   ", clientId='" + clientId + '\'' +
                   ", topic='" + topic + '\'' +
                   ", remoteAddress='" + remoteAddress + '\'' +
                   ", remotePort=" + remotePort +
                   ", createTime=" + createTime +
                   ", localAddress='" + localAddress + '\'' +
                   ", localPort=" + localPort +
                   '}';
        }


        public enum EType
        {
            SRV,
            SUB
        }
    }
}