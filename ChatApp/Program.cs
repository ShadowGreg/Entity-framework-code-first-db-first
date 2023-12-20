using System.Net;
using ChatApp.Services;
using ChatCommon.Abstracts;
using ChatNetwork.Services;

namespace ChatApp;

public static class Program {
    public static async Task Main(string[] args) {
        if (args[0].Equals("UDP SERVER")) {
            var netMsgSource = new UdpMessageSourceServer();
            var server = new Server<IPEndPoint>(netMsgSource);
            await server.Start();
        }
        else if (args[0].Equals("UDP CLIENT")) {
            var netMsgSource = new UdpMessageSourceClient();
            var client = new Client<IPEndPoint>(args[0], netMsgSource);
            await client.Start();
        }
        else if (args[0].Equals("NETMQ") && args[1].Equals("SERVER")) {
            var netMsgSource = new NetMqMessageSourceServer();
            var server = new Server<IPEndPoint>(netMsgSource);
            await server.Start();
        }
        else if (args[0].Equals("NETMQ") && args[1].Equals("CLIENT")) {
            IMessageSourceClient<IPEndPoint> netMsgSource = new NetMqMessageSourceClient();
            var client = new Client<IPEndPoint>(args[2], netMsgSource);
            await client.Start();
        }
    }
}