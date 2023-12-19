using System.Net;
using ChatCommon.Core.Entities;

namespace ChatNetwork.Abstracts;

public interface IClient<in T> {
    Task Conform(NetMessage messageResource, T remoteEndPoint);
    Task ClientListener();
    Task Register(T remoteEndPoint);
    Task ClientSender();
    Task Start();
    Task Stop();
}