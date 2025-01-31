// See https://aka.ms/new-console-template for more information
using Logic;
using System;

SpectreConsoleVisual game = new SpectreConsoleVisual();
Console.BackgroundColor = ConsoleColor.White;


var gc = GameBuilder.TestGame(game);


game.Play(gc);

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
            var key = AnsiConsole.Console.Input.ReadKey(false);
            if (key. == ConsoleKey.UpArrow)
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
            else if (key.Key == ConsoleKey.Q)
            {
                gc.currentPlayerActivatehability(0);
            }
            else if (key.Key == ConsoleKey.Spacebar)
            {
                gc.nextTurn();
            }

            else
            {
                break;
            }
        }
    }
    public void Move(GameCenter gameCenter, int i, int j, int x, int y, Player player)
    {
        AnsiConsole.Markup($"[blue] mov {i}, {j} -> {x}, {y} : {player.ToString()}[/]");
        // System.Console.WriteLine($"mov {i}, {j} -> {x}, {y} : {player.ToString()}");
    }

    public void NextTurn(GameCenter gameCenter)
    {
        AnsiConsole.Markup("[blue] Next Turn[/]");
        // System.Console.WriteLine("<next turn>");
    }

    public void Winning(GameCenter gameCenter, Player player)
    {
        if (player.isWinner)
        {
            //     AnsiConsole.Markup($"[blue] {player} es el ganador!![/]");
            // Console.WriteLine($"{player} es el ganador!!!");
        }
        else
        {
            // System.Console.WriteLine("Empate");
        }
    }
    public void Refresh(GameCenter gameCenter)
    {
        AnsiConsole.Console.Clear(true);
        var player = gameCenter.GetCurrentPlayer();
        var visible = gameCenter.board.PlayerVisibility(player);

        // string s = "";
        // s += $":> Jugador actual: {player.name} s({player.speed - gameCenter.currentMove}) v({player.visibility}) \n";

        // string table = "";
        // string topSeparator = "";
        // int n = gameCenter.board.n;
        // for (int i = 0; i < n + 2; i++)
        //     topSeparator += '_';
        // string bottomSeparator = "";
        // for (int i = 0; i < n + 2; i++)
        //     bottomSeparator += '-';
        // for (int i = 0; i < n; i++)
        // {
        //     string column = "|";
        //     for (int j = 0; j < n; j++)
        //     {
        //         if (visible[i, j])
        //             column += gameCenter.board.matrix[i, j].ToString();
        //         else column += "*";
        //     }
        //     column += "|\n";
        //     table += column;
        // }
        // s += topSeparator + "\n" + table + bottomSeparator + "\n";
        // System.Console.WriteLine(s);
        // Create a table
        var table = new Table();

        // Add some columns
        table.AddColumn("Foo");
        table.AddColumn(new TableColumn("Bar").Centered());

        // Add some rows
        table.AddRow("Baz", "[green]Qux[/]");
        table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));

        // Render the table to the console
        AnsiConsole.Write(table);
    }
    public void canNotMove(GameCenter gameCenter, Player player, int direction)
    {
        // System.Console.WriteLine($"El user {player} no se puede mover para {direction}");
    }

    public void canNotCallHability(GameCenter gameCenter, Player player, int index, Hability? hability)
    {
        // System.Console.WriteLine($"El player {player} no se puede ejecutar la habilidad {index}: {hability}");
    }

    public void callHability(GameCenter gameCenter, Player player, int index, Hability? hability)
    {
        // System.Console.WriteLine($"El player {player} ejecutó la habilidad {index}: {hability}");

    }

}


