using System.Net;
using ChatApp.Services;
using ChatNetwork.Services;

namespace ChatApp;

public static class Program {
    public static async Task Main(string[] args) {
        if (args[0].Equals("SERVER")) {
            var netMsgSource = new UdpMessageSourceServer();
            var server = new Server<IPEndPoint>(netMsgSource);
            await server.Start();
        }
        else {
            var netMsgSource = new UdpMessageSourceClient();
            var client = new Client<IPEndPoint>(args[0], netMsgSource);
            await client.Start();
        }
    }
}