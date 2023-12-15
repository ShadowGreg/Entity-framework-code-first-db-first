using System.Net;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Seminars.Abstracts;
using Seminars.Models;

namespace Seminars.Services;

public class Server {
    private readonly IMessageSource _messageSource;
    private readonly Dictionary<string, IPEndPoint> _clients;
    private IPEndPoint _ep;

    public Server() {
        _messageSource = new UdpMessageSource();
        _clients = new Dictionary<string, IPEndPoint>();
        _ep = new IPEndPoint(IPAddress.Any, 0);
    }

    public async Task Register(NetMessage netMessage) {
        Console.WriteLine($" Message register from {netMessage.NickNameFrom} to {netMessage.NickNameTo}");

        if (_clients.TryAdd(netMessage.NickNameFrom, netMessage.EndPoint)) {
            using (ChartContext context = new ChartContext()) {
                context.Users.Add(
                    new User() { FullName = netMessage.NickNameFrom }
                );
                await context.SaveChangesAsync();
            }
        }
    }

    private async Task RelyMessage(NetMessage netMessage) {
        if (_clients.TryGetValue(netMessage.NickNameTo!, out _ep!)) {
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

            await _messageSource.SentAsync(netMessage, _ep!);

            Console.WriteLine($"Message sent from {netMessage.NickNameFrom} to {netMessage.NickNameTo}");
        }
        else {
            Console.WriteLine("Client not found");
        }
    }

    private async Task Confirmation(int? id) {
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

        while (true) {
            try {
                var message = _messageSource.ReceivedAsync(_ep).Result;
                Console.WriteLine(message.ToString());

                await ProcessMessage(message);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}