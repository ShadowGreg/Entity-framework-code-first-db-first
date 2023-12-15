using System.Net;
using Seminars.Abstracts;
using Seminars.Models;

namespace ServerTests; 

public class MockMessageSource: IMessageSource {
    public Task SentAsync(NetMessage message, IPEndPoint endPoint) {
        throw new NotImplementedException();
    }

    public Task<NetMessage> ReceivedAsync(IPEndPoint endPoint) {
        throw new NotImplementedException();
    }
}