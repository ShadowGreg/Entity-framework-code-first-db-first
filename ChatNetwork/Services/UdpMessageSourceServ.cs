using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatNetwork.Services;

public class UdpMessageSourceServ: IMessageSource<IPEndPoint> {
    private readonly UdpClient _udpClient;
    

    public UdpMessageSourceServ(int port = 12345) {
        _udpClient = new UdpClient(port);
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