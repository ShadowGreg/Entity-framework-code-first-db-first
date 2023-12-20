using System.Net;
using NetMQ;
using NetMQ.Sockets;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatNetwork.Services
{
    public class NetMqMessageSourceClient : IMessageSourceClient<IPEndPoint>
    {
        private readonly RequestSocket _requestSocket;
        private readonly IPEndPoint _serverEndpoint;

        public NetMqMessageSourceClient()
        {
            _requestSocket = new RequestSocket();
            _requestSocket.Connect("tcp://localhost:12345");
            if (_requestSocket.Options.LastEndpoint != null)
                _serverEndpoint = ParseEndpoint(_requestSocket.Options.LastEndpoint);
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

        public IPEndPoint GetServer() {
            return _serverEndpoint;
        }

        public IPEndPoint CopyEndPoint(IPEndPoint t)
        {
            return new IPEndPoint(t.Address, t.Port);
        }
        
        private IPEndPoint ParseEndpoint(string endpoint)
        {
            string[] parts = endpoint.Split(':');
            if (parts.Length == 2 && IPAddress.TryParse(parts[0], out IPAddress address) && int.TryParse(parts[1], out int port))
            {
                return new IPEndPoint(address, port);
            }
            throw new ArgumentException("Invalid endpoint format.");
        }
    }
}