using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;
using Microsoft.VisualBasic;

namespace ChatNetwork.Services;

public class UdpMessageSourceClient: IMessageSourceClient<IPEndPoint> {
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _remoteEndPoint;
    JsonSerializerOptions options = new JsonSerializerOptions();
   


    public UdpMessageSourceClient(string ip = "172.0.0.1", int port = 0) {
        options.Converters.Add(new IPEndPointConverter());
        _udpClient = new UdpClient();
        _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
        _remoteEndPoint = new IPEndPoint(IPAddress.Parse("172.0.0.1"), 12345);
    }

    public void Send(NetMessage message, IPEndPoint toAddr) {
        message.Command = Command.Register;
        message.Text = string.Empty;
        var serializedMessage = JsonSerializer.Serialize(message, options);
        byte[] buffer = Encoding.UTF8.GetBytes(serializedMessage);
        _udpClient.Send(buffer, buffer.Length, toAddr);
    }

    public NetMessage Receive(ref IPEndPoint fromAddr) {
        byte[] data = _udpClient.Receive(ref fromAddr);
        string str = Encoding.UTF8.GetString(data);
        var deserializedMessage = JsonSerializer.Deserialize<NetMessage>(str, options);

        return deserializedMessage ?? new NetMessage();
    }

    public IPEndPoint CreateNewEndPoint() {
        return new IPEndPoint(IPAddress.Any, 0);
    }

    public IPEndPoint GetServer() {
        return _remoteEndPoint;
    }
}