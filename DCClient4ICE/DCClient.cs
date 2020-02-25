using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DC;
using DCClient4ICE_Client;

namespace DC
{
    public class DCClient
    {
        private static Dictionary<string, DefaultClient> _clientDic = new Dictionary<string, DefaultClient>();

        public static IDCClient Instance(string clientid = "")
        {
            if (_clientDic.ContainsKey(clientid)) return _clientDic[clientid];
            if (string.IsNullOrWhiteSpace(clientid))
            {
                clientid = DateTime.Now.ToFileTime().ToString();
            }

            DefaultClient tempClient = new DefaultClient(clientid);
            _clientDic.Add(clientid, tempClient);

            return _clientDic[clientid];
        }
    }
}