using Rock_Paper_Scissors.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network
{
    public class Host
    {
        private readonly Socket _listener;
        private Socket _client;

        private bool _started = false;

        public Host()
        {
            _listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    localAddr = address;
                    break;
                }
            }
            _listener.Bind(new IPEndPoint(localAddr, 13000));
        }
        
        public void Start()
        {
            Console.WriteLine("Server started at {0}.", _listener.LocalEndPoint);
            _listener.Listen();
            Console.WriteLine("Waiting for a client...");
            _client = _listener.Accept();
            Console.WriteLine("Client at {0} connected.", _client.LocalEndPoint);
            _started = true;
        }

        public void Stop()
        {
            _client.Disconnect(false);
            _listener.Close();
        }

        public byte[] ReceiveData()
        {
            byte[] data = new byte[256];
            _client.Receive(data);

            return data;
        }

        public void SendPacket(Packet packet)
        {
            byte[] data = packet.ToByteArray();
            _client.Send(data);
        }
    }
}
