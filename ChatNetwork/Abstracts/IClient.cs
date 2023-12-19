using System.Net;
using ChatCommon.Core.Entities;

namespace ChatNetwork.Abstracts;

public interface IClient {
    Task Conform(NetMessage messageResource, IPEndPoint remoteEndPoint);
    Task ClientListener();
    Task Register(IPEndPoint remoteEndPoint);
    Task ClientSender();
    Task Start();
    Task Stop();
}