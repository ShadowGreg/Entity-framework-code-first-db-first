using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatNetwork.Services;

public class UdpMessageSourceClient: IMessageSourceClient<IPEndPoint> {
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _remoteEndPoint;


    public UdpMessageSourceClient(string ip = "172.0.0.1", int port = 0) {
        _udpClient = new UdpClient(5527);
        _remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    public void Send(NetMessage message, IPEndPoint toAddr) {
        byte[] buffer = Encoding.UTF8.GetBytes(message.SerializeMessageToJson());
        _udpClient.Send(buffer, buffer.Length, toAddr);
    }

    public NetMessage Receive(ref IPEndPoint fromAddr) {
        byte[] data = _udpClient.Receive(ref fromAddr);
        string str = Encoding.UTF8.GetString(data);
        return NetMessage.DeserializeMessgeFromJSON(str) ?? new NetMessage();
    }

    public IPEndPoint CreateNewEndPoint() {
        return new IPEndPoint(IPAddress.Any, 0);
    }

    public IPEndPoint GetServer() {
        return _remoteEndPoint;
    }
}