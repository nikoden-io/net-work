using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace Receiver;

internal class Program
{
    private static async Task Main(string[] args)
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

        var receiverSequenceNumber = (ushort)new Random().Next(1, ushort.MaxValue);

        // Wait for handshake
        Console.WriteLine("Waiting for handshake...");
        var isConnected = await ConnectionManager.WaitForHandshake(udpClient, receiverSequenceNumber);

        if (isConnected)
        {
            Console.WriteLine("Handshake successful.");
            // Proceed to receive data
            var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);

            var result = await udpClient.ReceiveAsync();
            var packet = Packet.Deserialize(result.Buffer);

            var message = Encoding.UTF8.GetString(packet.Data);
            Console.WriteLine($"Received message: {message}");
        }
        else
        {
            Console.WriteLine("Handshake failed.");
        }
    }
}