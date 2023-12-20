using System.Net;
using NetMQ;
using NetMQ.Sockets;
using System.Text;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatNetwork.Services
{
    public class NetMqMessageSourceServer : IMessageSourceServer<IPEndPoint>
    {
        private readonly ResponseSocket _responseSocket;

        public NetMqMessageSourceServer()
        {
            _responseSocket = new ResponseSocket();
            _responseSocket.Bind("tcp://*:12345");
        }

        public void Send(NetMessage message, IPEndPoint toAddr)
        {
            // Not applicable for server implementation
        }

        public NetMessage Receive(ref IPEndPoint fromAddr)
        {
            string jsonMessage = _responseSocket.ReceiveFrameString();
            return NetMessage.DeserializeMessgeFromJSON(jsonMessage) ?? new NetMessage();
        }

        public IPEndPoint CreateNewEndPoint()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public IPEndPoint CopyEndPoint(IPEndPoint t)
        {
            return new IPEndPoint(t.Address, t.Port);
        }
    }
}