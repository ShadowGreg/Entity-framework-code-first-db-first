using System.Net;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatApp.Services;

public class MessageSourceClient: IMessageSourceClient<IPEndPoint> {
    private readonly string _name;
    private readonly string _ip;
    private readonly int _port;
    public IMessageSource<IPEndPoint> _messageSource;
    private readonly IPEndPoint _remoteEndPoint;
    private bool flag = true;

    public MessageSourceClient(string name, string ip, IMessageSource<IPEndPoint> messageSource) {
        _name = name;
        _ip = ip;
        _port = 12345;
        _messageSource = messageSource;
    }

    public async Task Conform(NetMessage messageResource, IPEndPoint remoteEndPoint) {
        messageResource.Command = Command.Confirmation;
        string messageJson = messageResource.SerialazeMessageToJSON();
        await _messageSource.SentAsync(messageResource, remoteEndPoint);
    }

    public async Task ClientListener() {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
        while (true) {
            NetMessage messageResource =    await _messageSource.ReceivedAsync(remoteEndPoint);

            Console.WriteLine("Message received from: " + messageResource.NickNameFrom);
            Console.WriteLine(messageResource.Text);

            await Conform(messageResource, remoteEndPoint);
        }
    }

    public async Task Register(IPEndPoint remoteEndPoint) {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        await _messageSource.SentAsync(
            new NetMessage() {
                Command = Command.Register,
                NickNameFrom = _name,
                EndPoint = ep
            },
            remoteEndPoint);
    }

    public async Task ClientSender() {
        await Register(_remoteEndPoint);

        while (flag) {
            try {
                Console.WriteLine("Input address: ");
                string address = Console.ReadLine() ?? throw new InvalidOperationException();
                Console.WriteLine("Input message: ");
                string text = Console.ReadLine() ?? throw new InvalidOperationException();
                await _messageSource.SentAsync(
                    new NetMessage() {
                        Command = Command.Message,
                        Text = text,
                        NickNameFrom = _name,
                        NickNameTo = address,
                        EndPoint = _remoteEndPoint
                    },
                    _remoteEndPoint);
                Console.WriteLine("Message sent");
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }


    public async Task Start() {
        await ClientListener();
        await ClientSender();
    }

    public Task Stop() {
        flag = false;
        return Task.CompletedTask;
    }

    public void Send(NetMessage message, IPEndPoint toAddr) {
        _messageSource.SentAsync(message, toAddr).Wait();
    }

    public NetMessage Receive(ref IPEndPoint fromAddr) {
        return  _messageSource.ReceivedAsync(fromAddr).Result;
    }

    public IPEndPoint CreateNewEndPoint() {
        return new IPEndPoint(IPAddress.Any, 0);
    }

    public IPEndPoint GetServer() {
        return _remoteEndPoint;
    }
}