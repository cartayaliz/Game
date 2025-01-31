
using Microsoft.VisualBasic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Logic
{
    public interface IVisual
    {
        public void Init(GameCenter gameCenter);
        public void Play(GameCenter gameCenter);
        public void NextTurn(GameCenter gameCenter);



        public void Winning(GameCenter gameCenter, Player player);
        public void Refresh(GameCenter gameCenter);
        public void Move(GameCenter gameCenter, int i, int j, int x, int y, Player player);

        public void canNotMove(GameCenter gameCenter, Player player, int direction);

        public void canNotCallHability(GameCenter gameCenter, Player player, int index, Hability? hability);

        public void callHability(GameCenter gameCenter, Player player, int index, Hability? hability);

    }

    class BasicConsoleVisual : IVisual
    {

        public void Init(GameCenter gameCenter)
        {
            System.Console.WriteLine("<init>");
            Refresh(gameCenter);

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
            // Console.Clear();
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

        public void canNotMove(GameCenter gameCenter, Player player, int direction)
        {
            System.Console.WriteLine($"El user {player} no se puede mover para {direction}");
        }

        public void canNotCallHability(GameCenter gameCenter, Player player, int index, Hability? hability)
        {
            System.Console.WriteLine($"El player {player} no se puede ejecutar la habilidad {index}: {hability}");
        }

        public void callHability(GameCenter gameCenter, Player player, int index, Hability? hability)
        {
            System.Console.WriteLine($"El player {player} ejecutó la habilidad {index}: {hability}");

        }
    }
    class SpectreConsoleVisual : IVisual
    {
        public Dictionary<string, string> mappedChar = new Dictionary<string, string>();

        public string prompt = "Start Game";

        public void Init(GameCenter gameCenter)
        {
            AnsiConsole.Markup("[underline red]Start game !!! \n[/]");
            Refresh(gameCenter);
        }
        private Dictionary<string, string> contentMap = new Dictionary<string, string>()
    {
        {"I", "[red]I[/]" },
        {"E", "[blue]E[/]" },
        {"F", "[green]F[/]" },
        {"A", "[purple]A[/]"},
        {"O", "[yellow]O[/]"},
        {"&", "[aqua]&[/]"},
        {"$", "[fuchsia]$[/]"},
        {"?", "[navyblue]:bell:[/]"},
        {"x", ":black_large_square:"},
        {"=", "[silver]=[/]"},
        {"2s", ":camera: "},
        {"1m", ":camera: "},
        {"2v", ":eyes: "}
    };
        public void Play(GameCenter gc)
        {
            while (!gc.finishedGame)
            {
                var key = AnsiConsole.Console.Input.ReadKey(true);
                if (!key.HasValue) continue;
                if (key.Value.Key == ConsoleKey.UpArrow)
                {
                    gc.currentPlayerAction(0);
                }
                else if (key.Value.Key == ConsoleKey.RightArrow)
                {
                    gc.currentPlayerAction(1);
                }
                else if (key.Value.Key == ConsoleKey.DownArrow)
                {
                    gc.currentPlayerAction(2);
                }
                else if (key.Value.Key == ConsoleKey.LeftArrow)
                {
                    gc.currentPlayerAction(3);
                }
                else if (key.Value.Key == ConsoleKey.Q)
                {
                    gc.currentPlayerActivatehability(0);
                }
                else if (key.Value.Key == ConsoleKey.Spacebar)
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
            prompt = $" {player} : {i}, {j} => {x}, {y} [/]";
            Refresh(gameCenter);
        }

        public void NextTurn(GameCenter gameCenter)
        {
            prompt = $"Next Turn: {gameCenter.GetCurrentPlayer().name}";
            Refresh(gameCenter);
        }

        public void Winning(GameCenter gameCenter, Player player)
        {
            if (player.isWinner)
            {
                AnsiConsole.Write($"{player} is the winner!![/]");
            }
            else
            {
                AnsiConsole.Write("Empate ");
            }
        }
        public void Refresh(GameCenter gameCenter)
        {
            AnsiConsole.Console.Clear(true);
            var player = gameCenter.GetCurrentPlayer();
            var visible = gameCenter.board.PlayerVisibility(player);

            int n = gameCenter.board.n;

            var table = new Table();
            table.HideHeaders();
            for (int i = 0; i < n; i++)
                table.AddColumn(i.ToString());
            for (int i = 0; i < n; i++)
            {
                var columns = new string[n];
                for (int j = 0; j < n; j++)
                {

                    if (visible[i, j])
                    {
                        var cellContent = gameCenter.board.matrix[i, j].ToString();
                        var column = contentMap.ContainsKey(cellContent)
                        ? contentMap[cellContent]
                        : $"[white]{cellContent}[/]";
                        columns[j] = column;
                    }
                    else columns[j] = $"[white]:black_square_button:[/]";
                }
                table.AddRow(columns);
            }

            table.Border(TableBorder.Rounded);

            var table2 = new Table();
            table2.AddColumn("Turn");
            table2.AddColumn("Player");
            table2.AddColumn("Visibility");
            table2.AddColumn("Speed");
            table2.AddColumn("Hability");
            table2.AddColumns("Active_Hability");
            table2.AddColumns("Hability_Off");

            var table3 = new Table();
            table3.AddColumn(":> " + prompt);

            for (int i = 0; i < gameCenter.players.Count; i++)
            {
                var p = gameCenter.players[i];
                var AH = "";
                var PH = "";
                var OH = "";

                foreach (var item in p.hability)
                {
                    string name = contentMap.GetValueOrDefault(item.ToString() + "", item.ToString() + "");
                    if (item.active) AH += name;
                    else if (item.canBeActive(p)) PH += name;
                    else OH += name;
                }


                var playerName = contentMap.ContainsKey(p.name.ToString())
                ? contentMap[p.name.ToString()]
                : $"[white]{p.name}[/]";
                int speed = p.speed;
                if (p.id == player.id) speed -= gameCenter.currentMove;
                table2.AddRow(p.id == player.id ? "[green] :check_mark: [/]" : "", playerName, p.visibility.ToString(), (speed).ToString(), AH, PH, OH);
            }


            AnsiConsole.Write(table);
            AnsiConsole.Write(table2);
            AnsiConsole.Write(table3);
        }
        public void canNotMove(GameCenter gameCenter, Player player, int direction)
        {
            prompt = $"El user {player.name} no se puede mover para {direction}";
        }

        public void canNotCallHability(GameCenter gameCenter, Player player, int index, Hability? hability)
        {
            prompt = $"El player {player.name} no se puede ejecutar la habilidad  {hability}";
        }

        public void callHability(GameCenter gameCenter, Player player, int index, Hability? hability)
        {
            prompt = $"El player {player.name} ejecutó la habilidad  {hability}";
        }

    }

}


