using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ChatCommon.Models.Entities;

public enum Command {
    Register,
    Message,
    Confirmation
}

public class NetMessage {
    public int? Id { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public string? NickNameFrom { get; set; }
    public string? NickNameTo { get; set; }

    public IPEndPoint? EndPoint { get; set; }

    public Command Command { get; set; }

    public string SerializeMessageToJson() {
        string temp = "";
        try {
            temp = JsonSerializer.Serialize(this);
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }

        return temp;
    }

    public static NetMessage? DeserializeMessgeFromJSON(string message) =>
        JsonSerializer.Deserialize<NetMessage>(message);

    public void PrintGetMessageFrom() {
        Console.WriteLine(ToString());
    }

    public override string ToString() {
        return $"{DateTime} \n Получено сообщение {Text} \n от {NickNameFrom}  ";
    }
}

public class IPEndPointConverter : JsonConverter<IPEndPoint>
{
    public override IPEndPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var ipAndPort = reader.GetString().Split(':');
        var ipAddress = IPAddress.Parse(ipAndPort[0]);
        var port = int.Parse(ipAndPort[1]);
        return new IPEndPoint(ipAddress, port);
    }

    public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.Address}:{value.Port}");
    }
}