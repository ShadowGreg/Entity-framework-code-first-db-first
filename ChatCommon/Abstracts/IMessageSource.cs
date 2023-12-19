using ChatCommon.Models.Entities;

namespace ChatCommon.Abstracts;

public interface IMessageSource<T> {
    Task SentAsync(NetMessage message, T endPoint);

    Task<NetMessage> ReceivedAsync(T endPoint);
   
}