using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Receiver <ListeningPort>");
                return;
            }

            int listeningPort = int.Parse(args[0]);

            UdpClient udpClient = new UdpClient(listeningPort);
            Console.WriteLine($"Receiver is listening on port {listeningPort}");

            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpClient.Receive(ref remoteEndpoint);

            string message = Encoding.UTF8.GetString(data);
            Console.WriteLine($"Received message: {message}");
        }
    }
}