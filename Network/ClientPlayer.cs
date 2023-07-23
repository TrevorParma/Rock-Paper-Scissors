using Rock_Paper_Scissors.Game;
using Rock_Paper_Scissors.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network
{
    public class ClientPlayer
    {
        private Client _client;

        // Packets
        private Packet _signalPacket = new();
        private ThrowPacket? _throwPacket = null;
        private ScoreUpdatePacket? _scoreUpdatePacket = null;
        private GameOverPacket? _gameOverPacket = null;
        private RematchPacket? _rematchPacket = null;

        public ClientPlayer()
        {
            _client = new();
        }

        public void JoinGame()
        {
            bool validIP;
            IPAddress address;
            do
            {
                Console.WriteLine("Enter the IP address of the host to join a game.");
                validIP = IPAddress.TryParse(Console.ReadLine(), out address);
            } while (!validIP);

            _client.Connect(address);
        }

        public void SendThrow()
        {
            _signalPacket = new(_client.ReceiveData());

            if (_signalPacket.Type == PacketType.ThrowRequest)
            {
                _throwPacket = new(Game.Manager.PromptThrow());
                _client.SendPacket(_throwPacket);
            }
            else
            {
                Console.WriteLine("Expected a throw request but received a packet of type {0} instead.", _signalPacket.Type);
            }
        }

        public void ReceiveScoreUpdate()
        {
            _scoreUpdatePacket = new(_client.ReceiveData());

            if (_scoreUpdatePacket.Type == PacketType.ScoreUpdate)
            {
                Console.WriteLine("The current score is {0} (host) to {1} (you).", _scoreUpdatePacket.Player1Score, _scoreUpdatePacket.Player2Score);

                _signalPacket = new(PacketType.ScoreUpdateResponse);
                _client.SendPacket(_signalPacket);
            }
            else
            {
                Console.WriteLine("Expected a score update but received a packet of type {0} instead.", _scoreUpdatePacket.Type);
            }
        }

        public bool ReceiveGameOver()
        {
            _gameOverPacket = new(_client.ReceiveData());

            if (_gameOverPacket.Type == PacketType.GameOver)
            {
                if (_gameOverPacket.GameOver)
                    Console.WriteLine("The game has ended! Player {0} is the winner!", _gameOverPacket.Winner);
            }
            else
            {
                Console.WriteLine("Expected a game over signal but received a packet of type {0} instead.", _gameOverPacket.Type);
            }

            _signalPacket = new(PacketType.GameOverResponse);
            _client.SendPacket(_signalPacket);

            return _gameOverPacket.GameOver;
        }

        public bool SendRematch()
        {
            _rematchPacket = new(_client.ReceiveData());

            if (_rematchPacket.Type == PacketType.Rematch)
            {
                if (_rematchPacket.Rematch)
                    _rematchPacket = new(Manager.PromptRematch());
                else
                    _rematchPacket = new(false);

                Console.WriteLine(_rematchPacket.Rematch);

                _client.SendPacket(_rematchPacket);
                return _rematchPacket.Rematch;
            }
            else
            {
                Console.WriteLine("Expected a rematch request but received a packet of type {0} instead.");
            }

            return false;
        }

        public void EndGame()
        {
            _client.Disconnect();
        }
    }
}
