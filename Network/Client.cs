using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Rock_Paper_Scissors.Network.Packets;

namespace Rock_Paper_Scissors.Network
{
    public class Client
    {
        private readonly Socket _socket;

        public Client()
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(IPAddress address)
        {
            IPEndPoint host = new(address, 13000);
            _socket.Connect(host);
        }

        public void Disconnect()
        {
            _socket.Disconnect(false);
        }

        public byte[] ReceiveData()
        {
            byte[] data = new byte[256];
            _socket.Receive(data);

            return data;
        }

        public void SendPacket(Packet packet)
        {
            byte[] data = packet.ToByteArray();
            _socket.Send(data);
        }
    }
}
