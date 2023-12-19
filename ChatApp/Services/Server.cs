using System.Net;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;
using ChatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Services;

public class Server<T> where T : class {
    private readonly IMessageSourceServer<T> _messageSource;
    public Dictionary<string, T> Clients { get; }
    private T _ep;
    private readonly CancellationTokenSource _tokenSource;


    public Server(IMessageSourceServer<T> messageSource) {
        _messageSource = messageSource;
        Clients = new Dictionary<string, T>();
        _ep = messageSource.CreateNewEndPoint();
        _tokenSource = new CancellationTokenSource();
    }

    public async Task Register(NetMessage netMessage) {
        Console.WriteLine($" Message register from {netMessage.NickNameFrom} to {netMessage.NickNameTo}");

        var castEndPoint = netMessage.EndPoint as T;
        if (Clients.TryAdd(netMessage.NickNameFrom, castEndPoint )) {
            using (ChartContext context = new ChartContext()) {
                context.Users.Add(
                    new User() { FullName = netMessage.NickNameFrom }
                );
                await context.SaveChangesAsync();
            }
        }
    }

    public async Task RelyMessage(NetMessage netMessage) {
        if (Clients.TryGetValue(netMessage.NickNameTo!, out _ep!)) {
            int id = 0;
            await using (var ctx = new ChartContext()) {
                var fromUser = await ctx.Users
                    .FirstOrDefaultAsync(x => x.FullName == netMessage.NickNameFrom);
                var toUser = await ctx.Users
                    .FirstOrDefaultAsync(x => x.FullName == netMessage.NickNameTo);
                var message = new Message
                    { UserFrom = fromUser, UserTo = toUser, Text = netMessage.Text, IsSent = false };

                ctx.Messages.Add(message);

                id = message.MessageId;
            }

            netMessage.Id = id;

            _messageSource.Send(netMessage, _ep!);

            Console.WriteLine($"Message sent from {netMessage.NickNameFrom} to {netMessage.NickNameTo}");
        }
        else {
            Console.WriteLine("Client not found");
        }
    }

    public async Task Confirmation(int? id) {
        Console.WriteLine("Message confirmed id= " + id);
        using (var ctx = new ChartContext()) {
            var message = await ctx.Messages
                .FirstOrDefaultAsync(x => x.MessageId == id);
            if (message != null) {
                message.IsSent = true;
                await ctx.SaveChangesAsync();
            }
        }
    }

    public async Task ProcessMessage(NetMessage netMessage) {
        switch (netMessage.Command) {
            case Command.Register:
                await Register(netMessage);
                break;

            case Command.Message:
                await RelyMessage(netMessage);
                break;

            case Command.Confirmation:
                await Confirmation(netMessage.Id);
                break;
        }
    }


    public async Task Start() {
        Console.WriteLine("Server started, and listening for messages...");

        while (!_tokenSource.IsCancellationRequested) {
            try {
                var message = _messageSource.Receive(ref _ep);
                Console.WriteLine(message.ToString());

                await ProcessMessage(message);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public Task Stop() {
        _tokenSource.Cancel();
        return Task.CompletedTask;
    }

    public void Send(NetMessage message, T toAddr) {
        _messageSource.Send(message, toAddr);
    }

    public NetMessage Receive(ref T fromAddr) {
        return _messageSource.Receive(ref fromAddr);
    }
}