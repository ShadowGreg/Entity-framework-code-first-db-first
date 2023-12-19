using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatCommon.Core.Entities;
using ChatNetwork.Abstracts;

namespace ChatNetwork.Services;

public class UdpMessageSource: IMessageSource {
    private readonly UdpClient _udpClient;

    public UdpMessageSource() {
        _udpClient = new UdpClient(12345);
    }

    public async Task SentAsync(NetMessage message, IPEndPoint endPoint) {
        byte[] bytes = Encoding.UTF8.GetBytes(message.SerialazeMessageToJSON());
        await _udpClient.SendAsync(bytes, bytes.Length, endPoint);
    }

    public async Task<NetMessage> ReceivedAsync(IPEndPoint endPoint) {
        var bytes = (await _udpClient.ReceiveAsync()).Buffer;
        string message = Encoding.UTF8.GetString(bytes);
        return NetMessage.DeserializeMessgeFromJSON(message) ?? new NetMessage();
    }
}