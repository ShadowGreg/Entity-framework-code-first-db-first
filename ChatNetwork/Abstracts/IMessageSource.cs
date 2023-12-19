using System.Net;
using ChatCommon.Core.Entities;

namespace ChatNetwork.Abstracts;

public interface IMessageSource {
    Task SentAsync(NetMessage message, IPEndPoint endPoint);

    Task<NetMessage> ReceivedAsync(IPEndPoint endPoint);
}