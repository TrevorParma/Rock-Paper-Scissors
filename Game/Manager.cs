using Rock_Paper_Scissors.Network.Packets;
using Rock_Paper_Scissors.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Game
{
    public class Manager
    {
        public int Player1Points { get; private set; }
        public int Player2Points { get; private set; }
        public int PointsToWin { get; private set; }

        public bool GameOver 
        { 
            get
            {
                if (Player1Points >= PointsToWin || Player2Points >= PointsToWin)
                    return true;
                return false;
            }
        }

        public byte GameWinner
        {
            get
            {
                if (Player1Points >= PointsToWin)
                    return 1;
                else if (Player2Points >= PointsToWin)
                    return 2;
                return 0;
            }
        }

        private readonly RoundResult[] _rockComparison = { RoundResult.Tie, RoundResult.Loss, RoundResult.Win };
        private readonly RoundResult[] _paperComparison = { RoundResult.Win, RoundResult.Tie, RoundResult.Loss };
        private readonly RoundResult[] _scissorComparison = { RoundResult.Loss, RoundResult.Win, RoundResult.Tie };

        public Manager() : this(3) { }

        public Manager(int pointsToWin)
        {
            Reset();
            PointsToWin = pointsToWin;
        }

        public void Reset()
        {
            Player1Points = 0;
            Player2Points = 0;
        }

        public void DetermineWinner(PlayerThrow p1Throw, PlayerThrow p2Throw)
        {
            RoundResult result = RoundResult.Tie;
            switch (p1Throw)
            {
                case PlayerThrow.Rock:
                    result = _rockComparison[(int)p2Throw];
                    break;
                case PlayerThrow.Paper:
                    result = _paperComparison[(int)p2Throw];
                    break;
                case PlayerThrow.Scissors:
                    result = _scissorComparison[(int)p2Throw];
                    break;
            }

            switch (result)
            {
                case RoundResult.Win:
                    Console.WriteLine("Player 1 wins the round!");
                    Player1Points++;
                    break;
                case RoundResult.Loss:
                    Console.WriteLine("Player 2 wins the round!");
                    Player2Points++;
                    break;
                case RoundResult.Tie:
                    Console.WriteLine("The round was a tie!");
                    break;
            }
        }

        public static PlayerThrow PromptThrow()
        {
            string selection = "";
            do
            {
                Console.WriteLine("What do you throw? Rock, paper, or scissors?");
                selection = Console.ReadLine().ToLower();
            } while (selection != "rock" && selection != "paper" && selection != "scissors");

            PlayerThrow pThrow = PlayerThrow.Rock;
            switch (selection)
            {
                case "rock":
                    return PlayerThrow.Rock;
                case "paper":
                    return PlayerThrow.Paper;
                default:
                    return PlayerThrow.Scissors;
            }
        }

        public static bool PromptRematch()
        {
            string selection = "";
            do
            {
                Console.WriteLine("Play again?");
                selection = Console.ReadLine().ToLower();
            } while (selection != "yes" && selection != "no");

            if (selection == "yes")
                return true;

            return false;
        }

        public void PromptPointsToWin()
        {
            string pointStr = "";
            bool pointsValid = false;
            int points = 0;
            do
            {
                Console.WriteLine("Enter how many points are needed to win.");
                pointStr = Console.ReadLine();
                pointsValid = int.TryParse(pointStr, out points);
            } while (!pointsValid);

            PointsToWin = Math.Abs(points);
        }
    }
}
