using System.Net;
using ChatApp.Services;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ServerTests;

public class MockMessageSource: IMessageSource<IPEndPoint> {
    private Queue<NetMessage> _messages = new();
    private MessageSourceServer _messageSourceServer;
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
            await _messageSourceServer.Stop();
            return null;
        }

        return _messages.Dequeue();
    }

    public void SetServer(MessageSourceServer messageSourceServer) {
        _messageSourceServer = messageSourceServer;
    }
}