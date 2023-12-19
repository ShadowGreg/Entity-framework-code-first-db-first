using System.Net;
using ChatApp.Services;
using ChatCommon.Abstracts;
using ChatCommon.Models.Entities;
using ChatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerTests;

public class ServerTests {
    private IMessageSource<IPEndPoint> _messageSource;
    private MessageSourceServer _messageSourceServer;

    [SetUp]
    public void Setup() {
        _messageSource = new MockMessageSource();
        _messageSourceServer = new MessageSourceServer(_messageSource);
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
        await _messageSourceServer.Register(netMessage);

        // Assert
        Assert.IsTrue(_messageSourceServer.Clients.ContainsKey(netMessage.NickNameFrom));
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
        _messageSourceServer.Clients.Add(netMessage.NickNameTo, netMessage.EndPoint);

        // Act
        await _messageSourceServer.RelyMessage(netMessage);

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
        await _messageSourceServer.Confirmation(messageId);

        // Assert
        using (var context = new ChartContext()) {
            var message = await context.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            Assert.IsTrue(message?.IsSent == true);
        }
    }
}