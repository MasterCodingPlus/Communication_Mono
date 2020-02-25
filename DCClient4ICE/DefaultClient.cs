using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dc;
using Ice;
using Exception = System.Exception;

namespace DCClient4ICE_Client
{
    public class DefaultClient : IDCClient
    {
        public DefaultClient(string clientId)
        {
            this.clientId = clientId;
        }

        public ActResult act(string type, string dest, byte[] data)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    var bytes = twoway.actDest(dest, type, data);
                    return new ActResult() {ResultData = bytes};
                }
            }
            catch (Exception exception)
            {
                return new ActResult() {Exception = exception};
            }
        }

        public ActResult act(string type, string dest, byte[] data, int timeoutSeconds)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    var task = twoway.actDestAsync(dest, type, data);
                    if (task.Wait(timeoutSeconds))
                    {
                        return new ActResult() {ResultData = task.Result};
                    }

                    return new ActResult() {Exception = new Exception("调用超时")};
                }
            }
            catch (Exception exception)
            {
                return new ActResult() {Exception = exception};
            }
        }

        public ActResult act(string type, byte[] data)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    var bytes = twoway.act(type, data);
                    return new ActResult() {ResultData = bytes};
                }
            }
            catch (Exception exception)
            {
                return new ActResult() {Exception = exception};
            }
        }

        public ActResult act(string type, byte[] data, int timeoutSeconds)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    var task = twoway.actAsync(type, data);
                    if (task.Wait(timeoutSeconds))
                    {
                        return new ActResult() {ResultData = task.Result};
                    }

                    return new ActResult() {Exception = new Exception("调用超时")};
                }
            }
            catch (Exception exception)
            {
                return new ActResult() {Exception = exception};
            }
        }

        public void actAsync(string type, string dest, byte[] data, Action<ActResult> callback)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    var task = twoway.actDestAsync(dest, type, data);
                    task.ContinueWith((taskData => callback.Invoke(new ActResult() {ResultData = taskData.Result})));
                }
            }
            catch (Exception exception)
            {
                callback.Invoke(new ActResult() {Exception = exception});
            }
        }

        public void actAsync(string type, byte[] data, Action<ActResult> callback)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    var task = twoway.actAsync(type, data);
                    task.ContinueWith((taskData => callback.Invoke(new ActResult() {ResultData = taskData.Result})));
                }
            }
            catch (Exception exception)
            {
                callback.Invoke(new ActResult() {Exception = exception});
            }
        }

        public void Dispose()
        {
        }

        public void pub(string type, byte[] data)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    twoway.pub(type, data);
                }
            }
            catch (Exception exception)
            {
            }
        }

        public void pub(string type, byte[] data, int timeoutSeconds)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    twoway.pub(type, data);
                }
            }
            catch (Exception exception)
            {
            }
        }

        public void pubAsync(string type, byte[] data, Action<byte[]> callback)
        {
            try
            {
                using (var communicator = Ice.Util.initialize("config.client"))
                {
                    DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator
                        .propertyToProxy("dcServer.Proxy")
                        .ice_twoway().ice_secure(false));
                    if (twoway == null)
                    {
                        Console.Error.WriteLine("invalid proxy");
                    }

                    twoway.pubAsync(type, data);
                }
            }
            catch (Exception exception)
            {
            }
        }

        public void rcv(string type, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        public void snd(string type, string dest, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void snd(string type, string dest, byte[] data, int timeoutSeconds)
        {
            throw new NotImplementedException();
        }

        public void snd(string type, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void snd(string type, byte[] data, int timeoutSeconds)
        {
            throw new NotImplementedException();
        }

        public void sndAsync(string type, string dest, byte[] data, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        public void sndAsync(string type, byte[] data, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        private Communicator communicator;
        public string clientId;
        private string splitStr = "____";

        public void srv(string type, Func<string, byte[], byte[]> callback)
        {
            if (communicator == null)
            {
                communicator = Ice.Util.initialize("config.client");
            }

            DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator.propertyToProxy("dcServer.Proxy")
                .ice_twoway().ice_secure(false));
            if (twoway == null)
            {
                Console.Error.WriteLine("invalid proxy");
            }

            var adapter = communicator.createObjectAdapter("Callback.Client");
            adapter.add(new CallbackSrvReceiverI(callback),
                Ice.Util.stringToIdentity("callbackReceiverSrv" + clientId));
            adapter.activate();

            var receiver =
                SrvCallbackReceiverPrxHelper.uncheckedCast(
                    adapter.createProxy(Ice.Util.stringToIdentity("callbackReceiverSrv" + clientId)));

            twoway.srv(clientId, type + splitStr + clientId, receiver);
        }

        public void sub(string type, Action<byte[]> callback)
        {
            if (communicator == null)
            {
                communicator = Ice.Util.initialize("config.client");
            }

            DcClientIcePrx twoway = DcClientIcePrxHelper.checkedCast(communicator.propertyToProxy("dcServer.Proxy")
                .ice_twoway().ice_secure(false));
            if (twoway == null)
            {
                Console.Error.WriteLine("invalid proxy");
            }

            var adapter = communicator.createObjectAdapter("Callback.Client");
            adapter.add(new CallbackSubReceiverI(callback),
                Ice.Util.stringToIdentity("callbackReceiverSub" + clientId));
            adapter.activate();

            var receiver =
                SrvCallbackReceiverPrxHelper.uncheckedCast(
                    adapter.createProxy(Ice.Util.stringToIdentity("callbackReceiverSub" + clientId)));

            twoway.srv(clientId, type + splitStr + clientId, receiver);
        }

        public void sub(string type, string publisher, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        public byte[] take(string type)
        {
            throw new NotImplementedException();
        }

        public byte[] take(string type, string publisher)
        {
            throw new NotImplementedException();
        }

        public void unRcv(string type, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        public void unSrv(string type, Func<string, byte[], byte[]> callback)
        {
            throw new NotImplementedException();
        }

        public void unSub(string type, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        public void unSub(string type, string publisher, Action<byte[]> callback)
        {
            throw new NotImplementedException();
        }

        class CallbackSrvReceiverI : SrvCallbackReceiverDisp_
        {
            private Func<string, byte[], byte[]> callbackFunc;

            public CallbackSrvReceiverI(Func<string, byte[], byte[]> callback)
            {
                this.callbackFunc = callback;
            }

            public override byte[] callback(string extInfo, byte[] bytes, Current current = null)
            {
                return this.callbackFunc?.Invoke(extInfo, bytes);
            }
        }

        class CallbackSubReceiverI : SubCallbackReceiverDisp_
        {
            private Action<byte[]> callbackFunc;

            public CallbackSubReceiverI(Action<byte[]> callback)
            {
                this.callbackFunc = callback;
            }

            public override void callback(byte[] bytes, Current current = null)
            {
                this.callbackFunc?.Invoke(bytes);
            }
        }
    }
}