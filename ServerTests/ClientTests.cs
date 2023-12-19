using System.Net;
using ChatApp.Services;
using ChatCommon.Models.Entities;

namespace ServerTests;

public class ClientTests {
    [Test]
    public async Task ClientListener_ShouldReceiveAndConfirmMessages() {
        // Arrange
        var fakeMessageSource = new MockMessageSource();
        var client = new MessageSourceClient("John", "127.0.0.1", fakeMessageSource);

        // Act
        client.ClientListener();

        var message = new NetMessage() {
            NickNameFrom = "Alice",
            Text = "Hello, John!"
        };
        await fakeMessageSource.SentAsync(message, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));

        // Assert
        Assert.That(fakeMessageSource.LastReceivedMessage.NickNameFrom, Is.EqualTo("Alice"));
        Assert.That(fakeMessageSource.LastReceivedMessage.Text, Is.EqualTo("Hello, John!"));
        Assert.That(fakeMessageSource.LastReceivedMessage.Command, Is.EqualTo(Command.Confirmation));
    }

    [Test]
    public async Task ClientSender_ShouldSendMessages() {
        // Arrange
        var fakeMessageSource = new MockMessageSource();
        var client = new MessageSourceClient("John", "127.0.0.1", fakeMessageSource);

        // Act
        var senderTask = async () => await client.ClientSender();
        var message = new NetMessage() {
            NickNameFrom = "John",
            Text = "Hello, Alice!"
        };
        await fakeMessageSource.ReceivedAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
        await fakeMessageSource.SentAsync(message, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
        await client.Stop();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(fakeMessageSource.LastReceivedMessage.NickNameFrom, Is.EqualTo("John"));
            Assert.That(fakeMessageSource.LastReceivedMessage.Text, Is.EqualTo("Hello, Alice!"));
            Assert.That(fakeMessageSource.LastReceivedMessage.Command, Is.EqualTo(Command.Confirmation));
        });
    }
}