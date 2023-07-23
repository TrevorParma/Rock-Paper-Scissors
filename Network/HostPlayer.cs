using Rock_Paper_Scissors.Game;
using Rock_Paper_Scissors.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network
{
    public class HostPlayer
    {
        public Manager GameManager { get; init; }

        private readonly Host _host;

        // Packets
        private Packet _signalPacket = new();
        private ThrowPacket? _throwPacket = null;
        private ScoreUpdatePacket? _scorePacket = null;
        private GameOverPacket? _gameOverPacket = null;
        private RematchPacket? _rematchPacket = null;

        public HostPlayer()
        {
            _host = new();
            _host.Start();
            GameManager = new();
        }

        public void StartGame()
        {
            GameManager.Reset();
            GameManager.PromptPointsToWin();
        }

        public PlayerThrow ReceiveClientThrow()
        {
            // Send the client a throw request.
            _host.SendPacket(new(PacketType.ThrowRequest));

            // Receive the client's throw.
            _throwPacket = new(_host.ReceiveData());
            if (_throwPacket.Type == PacketType.ThrowSend)
                return _throwPacket.Throw;

            Console.WriteLine("Expected a client throw packet but received a packet of type {0} instead. Defaulted to rock.");
            return PlayerThrow.Rock;
        }

        public void SendScoreUpdate()
        {
            // Construct and send the packet.
            _scorePacket = new(GameManager.Player1Points, GameManager.Player2Points);
            _host.SendPacket(_scorePacket);

            // Wait for the client's response.
            _signalPacket = new(_host.ReceiveData());
            if (_signalPacket.Type != PacketType.ScoreUpdateResponse)
                Console.WriteLine("Expected a score update response but received a packet of type {0} instead.", _signalPacket.Type);
        }

        public void SendGameOver()
        {
            // Construct and send the packet.
            _gameOverPacket = new(GameManager.GameOver, GameManager.GameWinner);
            _host.SendPacket(_gameOverPacket);

            // Wait for the client's response.
            _signalPacket = new(_host.ReceiveData());
            if (_signalPacket.Type != PacketType.GameOverResponse)
                Console.WriteLine("Expected a game over response but received a packet of type {0} instead.", _signalPacket.Type);
        }

        public bool DetermineRematch()
        {
            // Construct and send the packet.
            _rematchPacket = new(Manager.PromptRematch());
            _host.SendPacket(_rematchPacket);

            if (!_rematchPacket.Rematch)
                return false;

            // Wait for the client's response.
            Console.WriteLine("Waiting for client...");
            _rematchPacket = new(_host.ReceiveData());
            if (_rematchPacket.Type == PacketType.Rematch)
                return _rematchPacket.Rematch;

            Console.WriteLine("Expected a rematch response but received a packet of type {0} instead. Defaulting to false.", _rematchPacket.Type);
            return false;
        }

        public void EndGame()
        {
            _host.Stop();
        }
    }
}
