// See https://aka.ms/new-console-template for more information
using Rock_Paper_Scissors.Game;
using Rock_Paper_Scissors.Network;
using Rock_Paper_Scissors.Network.Packets;
using System.Net;

BaseLoop();

void BaseLoop()
{
    string selection = "";
    do
    {
        do
        {
            Console.WriteLine("Start the game as a host by inputting 'host' or a client by inputting 'join'. Enter 'exit' to close the program.");
            selection = Console.ReadLine().ToLower();
        } while (selection != "host" && selection != "join" && selection != "exit");

        switch (selection)
        {
            case "host":
                HostGame();
                break;
            case "join":
                ClientGame();
                break;
        }
    } while (selection != "exit");
}

void HostGame()
{
    HostPlayer player = new();

    bool gameOver = false;

    PlayerThrow hostThrow;
    PlayerThrow clientThrow;

    player.StartGame();

    while (!gameOver)
    {
        hostThrow = Manager.PromptThrow();
        Console.WriteLine("Waiting for client...");
        clientThrow = player.ReceiveClientThrow();

        player.GameManager.DetermineWinner(hostThrow, clientThrow);

        player.SendScoreUpdate();

        player.SendGameOver();

        if (player.GameManager.GameOver)
        {
            if (player.DetermineRematch())
            {
                Console.WriteLine("The rematch has been accepted.");
                player.StartGame();
            }
            else
            {
                Console.WriteLine("The rematch has been declined.");
                gameOver = true;
            }
        }
    }

    player.EndGame();
}

void ClientGame()
{
    ClientPlayer player = new();

    bool gameOver = false;

    player.JoinGame();

    while (!gameOver)
    {
        Console.WriteLine("Waiting for host...");
        player.SendThrow();

        Console.WriteLine("Waiting for host...");
        player.ReceiveScoreUpdate();

        if (player.ReceiveGameOver())
            gameOver = !player.SendRematch();
    }

    player.EndGame();
}