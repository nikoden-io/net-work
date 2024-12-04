using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace Sender;

internal class Program
{
    private static async Task Main(string[] args)
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

        var senderSequenceNumber = (ushort)new Random().Next(1, ushort.MaxValue);

        var receiverEndpoint = new IPEndPoint(IPAddress.Parse(receiverIp), receiverPort);

        // Initiate handshake
        Console.WriteLine("Initiating handshake...");
        var isConnected = await ConnectionManager.InitiateHandshake(udpClient, receiverEndpoint, senderSequenceNumber);

        if (isConnected)
        {
            Console.WriteLine("Handshake successful.");
            // Proceed to send data
            // For now, send "Hello World" as before
            var message = "Hello World";

            var packet = new Packet
            {
                TotalDataSize = (ushort)Encoding.UTF8.GetByteCount(message),
                SequenceNumber = (ushort)(senderSequenceNumber + 1),
                Data = Encoding.UTF8.GetBytes(message)
            };

            var packetBytes = packet.Serialize();

            await udpClient.SendAsync(packetBytes, packetBytes.Length, receiverEndpoint);

            Console.WriteLine("Sent data packet to receiver.");

            // Send FIN Packet to initiate connection termination
            var finPacket = new Packet
            {
                FIN = true,
                SequenceNumber = (ushort)(senderSequenceNumber + 2),
                TotalDataSize = 0,
                Data = new byte[0]
            };

            var finBytes = finPacket.Serialize();
            await udpClient.SendAsync(finBytes, finBytes.Length, receiverEndpoint);

            Console.WriteLine("Sent FIN packet to receiver. Waiting for FIN-ACK.");

            // Wait for FIN-ACK
            var receiveTask = udpClient.ReceiveAsync();
            if (await Task.WhenAny(receiveTask, Task.Delay(3000)) == receiveTask)
            {
                var result = receiveTask.Result;
                var finAckPacket = Packet.Deserialize(result.Buffer);

                if (finAckPacket.FIN && finAckPacket.ACK)
                {
                    Console.WriteLine("Received FIN-ACK. Sending final ACK.");

                    // Send final ACK to complete termination
                    var finalAckPacket = new Packet
                    {
                        ACK = true,
                        SequenceNumber = (ushort)(senderSequenceNumber + 3),
                        TotalDataSize = 0,
                        Data = new byte[0]
                    };

                    var finalAckBytes = finalAckPacket.Serialize();
                    await udpClient.SendAsync(finalAckBytes, finalAckBytes.Length, receiverEndpoint);

                    Console.WriteLine("Connection terminated gracefully.");
                }
            }
            else
            {
                Console.WriteLine("Did not receive FIN-ACK. Connection termination failed.");
            }
        }
        else
        {
            Console.WriteLine("Handshake failed.");
        }
    }
}