using System.Net.Sockets;
using System.Text;
using Shared;

namespace Sender;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: Sender <ReceiverIP> <ReceiverPort> <InputFile>");
            return;
        }

        var receiverIp = args[0];
        var receiverPort = int.Parse(args[1]);
        var inputFile = args[2];

        var udpClient = new UdpClient();

        // For this step, we'll send "Hello World" placeholder data using the Packet structure
        var message = "Hello World";

        var packet = new Packet
        {
            TotalDataSize = (ushort)Encoding.UTF8.GetByteCount(message),
            SequenceNumber = 0,
            SYN = false,
            ACK = false,
            FIN = false,
            RST = false,
            Data = Encoding.UTF8.GetBytes(message)
        };

        var packetBytes = packet.Serialize();

        udpClient.Send(packetBytes, packetBytes.Length, receiverIp, receiverPort);

        Console.WriteLine("Sent 'Hello World' packet to receiver.");
    }
}