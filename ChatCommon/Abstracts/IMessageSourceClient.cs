using ChatCommon.Models.Entities;

namespace ChatCommon.Abstracts;

public interface IMessageSourceClient<T> {
    public void Send(NetMessage message, T toAddr);
    public NetMessage Receive(ref T fromAddr);
    public T CreateNewEndPoint();
    public T GetServer();
}