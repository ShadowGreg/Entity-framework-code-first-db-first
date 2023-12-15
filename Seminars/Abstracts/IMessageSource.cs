using System.Net;
using Seminars.Models;

namespace Seminars.Abstracts;

public interface IMessageSource {
    Task SentAsync(NetMessage message, IPEndPoint endPoint);

    Task<NetMessage> ReceivedAsync(IPEndPoint endPoint);
}