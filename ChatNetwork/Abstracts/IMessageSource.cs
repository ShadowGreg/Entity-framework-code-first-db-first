using System.Net;
using ChatCommon.Core.Entities;

namespace ChatNetwork.Abstracts;

public interface IMessageSource<T> {
    Task SentAsync(NetMessage message, T endPoint);

    Task<NetMessage> ReceivedAsync(T endPoint);
}