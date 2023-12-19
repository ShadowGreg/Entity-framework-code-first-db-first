using System.Net;
using ChatApp.Services;
using ChatCommon.Core.Entities;
using ChatDb.Models;
using ChatNetwork.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace ServerTests;

public class ServerTests {
    private IMessageSource _messageSource;
    private Server _server;

    [SetUp]
    public void Setup() {
        _messageSource = new MockMessageSource();
        _server = new Server(_messageSource);
    }

    [Test]
    public async Task Register_ShouldAddUserToClientsAndDatabase() {
        // Arrange
        var netMessage = new NetMessage {
            Command = Command.Register,
            NickNameFrom = "John",
            EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234)
        };

        // Act
        await _server.Register(netMessage);

        // Assert
        Assert.IsTrue(_server.Clients.ContainsKey(netMessage.NickNameFrom));
        using (var context = new ChartContext()) {
            var user = await context.Users.FirstOrDefaultAsync(u => u.FullName == netMessage.NickNameFrom);
            Assert.IsNotNull(user);
        }
    }

    [Test]
    public async Task RelyMessage_ShouldSendMessageToClientAndSaveToDatabase() {
        // Arrange
        var netMessage = new NetMessage {
            Command = Command.Message,
            NickNameFrom = "John",
            NickNameTo = "Jane",
            Text = "Hello",
            EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234)
        };
        _server.Clients.Add(netMessage.NickNameTo, netMessage.EndPoint);

        // Act
        await _server.RelyMessage(netMessage);

        // Assert
        using (var context = new ChartContext()) {
            var message = await context.Messages.FirstOrDefaultAsync(m => m.Text == netMessage.Text);
            Assert.IsNotNull(message);
        }
    }

    [Test]
    public async Task Confirmation_ShouldMarkMessageAsSentInDatabase() {
        // Arrange
        var messageId = 1;

        // Act
        await _server.Confirmation(messageId);

        // Assert
        using (var context = new ChartContext()) {
            var message = await context.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            Assert.IsTrue(message?.IsSent == true);
        }
    }
}