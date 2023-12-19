using ChatApp.Services;
using ChatNetwork.Services;

namespace ChatApp;

public static class Program {
    public static async Task Main(string[] args) {
        if (args[0].Equals("SERVER")) {
            var netMsgSource = new UdpMessageSourceServ();
            var server = new MessageSourceServer(netMsgSource);
            await server.Start();
        }
        else {
            var netMsgSource = new UdpMessageSourceClie();
            var client = new MessageSourceClient(args[0], "127.0.0.1", netMsgSource);
            await client.Start();
        }
    }
}