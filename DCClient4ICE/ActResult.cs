using System;

namespace DCClient4ICE_Client
{
    public class ActResult
    {
        public string Type { get; set; }
        public string Src { get; set; }
        public byte[] ResultData { get; set; }
        public int ActSn { get; set; }
        public Exception Exception { get; set; }
    }
}