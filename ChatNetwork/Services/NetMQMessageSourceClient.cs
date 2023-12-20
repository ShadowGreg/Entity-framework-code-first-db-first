using System.Net;
using NetMQ;
using NetMQ.Sockets;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatNetwork.Services
{
    public class NetMqMessageSourceClient : IMessageSourceServer<IPEndPoint>
    {
        private readonly RequestSocket _requestSocket;

        public NetMqMessageSourceClient()
        {
            _requestSocket = new RequestSocket();
            _requestSocket.Connect("tcp://localhost:12345");
        }

        public void Send(NetMessage message, IPEndPoint toAddr)
        {
            string jsonMessage = message.SerializeMessageToJson();
            _requestSocket.SendFrame(jsonMessage);
        }

        public NetMessage Receive(ref IPEndPoint fromAddr)
        {
            string jsonMessage = _requestSocket.ReceiveFrameString();
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