using ChatCommon.Models.Entities;

namespace ChatCommon.Abstracts;

public interface IMessageSourceServer<T> {
    void Send(NetMessage message, T toAddr);
    NetMessage Receive(ref T fromAddr);
    public T CreateNewEndPoint();
    public T CopyEndPoint(T t);
}