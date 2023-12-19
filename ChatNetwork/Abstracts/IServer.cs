using System.Net;
using ChatCommon.Core.Entities;

namespace ChatNetwork.Abstracts;

public interface IServer {
    Dictionary<string, IPEndPoint> Clients { get; }
    Task Register(NetMessage netMessage);
    Task RelyMessage(NetMessage netMessage);
    Task Confirmation(int? id);
    Task ProcessMessage(NetMessage netMessage);
    Task Start();
    Task Stop();
}