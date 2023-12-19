using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatNetwork.Services;

public class UdpMessageSourceServer: IMessageSourceServer<IPEndPoint> {
    private readonly UdpClient _udpClient;
    

    public UdpMessageSourceServer() {
        _udpClient = new UdpClient(12345);
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

    public IPEndPoint CopyEndPoint(IPEndPoint t) {
       return new IPEndPoint(t.Address, t.Port);
    }
}