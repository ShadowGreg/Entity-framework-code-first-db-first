using System.Net;
using ChatApp.Services;
using ChatCommon.Core.Entities;
using ChatNetwork.Abstracts;

namespace ServerTests;

public class MockMessageSource: IMessageSource {
    private Queue<NetMessage> _messages = new();
    private Server _server;
    private IPEndPoint _endPoint = new IPEndPoint(IPAddress.Any, 0);

    public NetMessage LastReceivedMessage { get; set; }

    public MockMessageSource() {
        _messages.Enqueue(new NetMessage() { Command = Command.Register, NickNameFrom = "Vasya" });
        _messages.Enqueue(new NetMessage() { Command = Command.Register, NickNameFrom = "Ulia" });
        _messages.Enqueue(new NetMessage()
            { Command = Command.Register, NickNameFrom = "Ulia", NickNameTo = "Vasya", Text = "Hello to Vasya" });
        _messages.Enqueue(new NetMessage()
            { Command = Command.Register, NickNameFrom = "Vasya", NickNameTo = "Ulia", Text = "Hello to Ulia" });
    }

    public async Task SentAsync(NetMessage message, IPEndPoint endPoint) {
        LastReceivedMessage = message;
        LastReceivedMessage.Command= Command.Confirmation;
        // throw new NotImplementedException();
    }

    public async Task<NetMessage> ReceivedAsync(IPEndPoint endPoint) {
        // throw new NotImplementedException();
        endPoint = _endPoint;
        if (_messages.Count == 0) {
            await _server.Stop();
            return null;
        }

        return _messages.Dequeue();
    }

    public void SetServer(Server server) {
        _server = server;
    }
}