// See https://aka.ms/new-console-template for more information
using Logic;
using Spectre.Console;

SpectreConsoleVisual game = new SpectreConsoleVisual();

var gc = GameBuilder.TestGame(game);

// game.Play(gc);

// System.Console.WriteLine("Hi !!!");
class SpectreConsoleVisual : IVisual
{
    public Dictionary<string, string> mappedChar = new Dictionary<string, string>();

    public void Init(GameCenter gameCenter)
    {
        AnsiConsole.Markup("[underline red]Start game !!! \n[/]");
        // System.Console.WriteLine("<init>");
        // Refresh(gameCenter);
        // Console.WriteLine(gameCenter.ToString());
    }
    public void Play(GameCenter gc)
    {
        while (!gc.finishedGame)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                gc.currentPlayerAction(0);
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                gc.currentPlayerAction(1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                gc.currentPlayerAction(2);
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                gc.currentPlayerAction(3);
            }
            // else if (key.Key == ConsoleKey.ctrl) shift fn alt tab    
            // {

            // }    
            else
            {
                break;
            }
        }

    }
    public void Move(GameCenter gameCenter, int i, int j, int x, int y, Player player)
    {
        System.Console.WriteLine($"mov {i}, {j} -> {x}, {y} : {player.ToString()}");
    }

    public void NextTurn(GameCenter gameCenter)
    {
        System.Console.WriteLine("<next turn>");
    }

    public void Winning(GameCenter gameCenter, Player player)
    {
        if (player.isWinner)
        {
            Console.WriteLine($"{player} es el ganador!!!");
        }
        else
        {
            System.Console.WriteLine("Empate");
        }
    }
    public void Refresh(GameCenter gameCenter)
    {
        Console.Clear();
        var player = gameCenter.GetCurrentPlayer();
        var visible = gameCenter.board.PlayerVisibility(player);

        string s = "";
        s += $":> Jugador actual: {player.name} s({player.speed - gameCenter.currentMove}) v({player.visibility}) \n";

        string table = "";
        string topSeparator = "";
        int n = gameCenter.board.n;
        for (int i = 0; i < n + 2; i++)
            topSeparator += '_';
        string bottomSeparator = "";
        for (int i = 0; i < n + 2; i++)
            bottomSeparator += '-';
        for (int i = 0; i < n; i++)
        {
            string column = "|";
            for (int j = 0; j < n; j++)
            {
                if (visible[i, j])
                    column += gameCenter.board.matrix[i, j].ToString();
                else column += "*";
            }
            column += "|\n";
            table += column;
        }
        s += topSeparator + "\n" + table + bottomSeparator + "\n";
        System.Console.WriteLine(s);
    }

}


