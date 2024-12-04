using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace Receiver;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: Receiver <ListeningPort> <OutputFile>");
            return;
        }

        var listeningPort = int.Parse(args[0]);
        var outputFile = args[1];

        var udpClient = new UdpClient(listeningPort);
        Console.WriteLine($"Receiver is listening on port {listeningPort}");

        var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);

        var data = udpClient.Receive(ref remoteEndpoint);

        var packet = Packet.Deserialize(data);

        var message = Encoding.UTF8.GetString(packet.Data);
        Console.WriteLine($"Received message: {message}");
    }
}