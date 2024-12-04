using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Sender <ReceiverIP> <ReceiverPort>");
                return;
            }

            string receiverIp = args[0];
            int receiverPort = int.Parse(args[1]);

            UdpClient udpClient = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes("Hello World");
            udpClient.Send(data, data.Length, receiverIp, receiverPort);

            Console.WriteLine("Sent 'Hello World' to receiver.");
        }
    }
}