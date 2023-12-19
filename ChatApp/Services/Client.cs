using System.Net;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;

namespace ChatApp.Services;

public class Client<T> {
    private readonly string _name;
    public readonly IMessageSourceClient<T> MessageSourceClient;
    private T _remoteEndPoint;

    private readonly string _ip;
    private readonly int _port;
    private bool flag = true;

    public Client(string name, IMessageSourceClient<T> messageSourceClient) {
        _name = name;
        MessageSourceClient = messageSourceClient;
        _remoteEndPoint = messageSourceClient.CreateNewEndPoint();
    }

    public void Conform(NetMessage messageResource, T remoteEndPoint) {
        messageResource.Command = Command.Confirmation;
        string messageJson = messageResource.SerializeMessageToJson();
        MessageSourceClient.Send(messageResource, remoteEndPoint);
    }

    public void ClientListener() {
        while (true) {
            NetMessage messageResource = MessageSourceClient.Receive(ref _remoteEndPoint);

            Console.WriteLine("Message received from: " + messageResource.NickNameFrom);
            Console.WriteLine(messageResource.Text);

            Conform(messageResource, _remoteEndPoint);
        }
    }

    public void Register(T remoteEndPoint) {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        MessageSourceClient.Send(
            new NetMessage() {
                Command = Command.Register,
                NickNameFrom = _name,
                EndPoint = ep.ToString()
            },
            _remoteEndPoint);
    }

    public void ClientSender() {
        Register(_remoteEndPoint);

        while (flag) {
            try {
                Console.WriteLine("Input address: ");
                if (_remoteEndPoint != null) {
                    string? address = Console.ReadLine() ?? _remoteEndPoint.ToString();
                    Console.WriteLine("Input message: ");
                    string text = Console.ReadLine() ?? throw new InvalidOperationException();
                    MessageSourceClient.Send(
                        new NetMessage() {
                            Command = Command.Message,
                            Text = text,
                            NickNameFrom = _name,
                            NickNameTo = address?? "127.0.0.1",
                            EndPoint = (_remoteEndPoint as IPEndPoint)?.ToString()
                        },
                        _remoteEndPoint);
                }

                Console.WriteLine("Message sent");
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }


    public async Task Start() {
        new Thread(ClientListener).Start();
        ClientSender();
    }

    public Task Stop() {
        flag = false;
        return Task.CompletedTask;
    }
}