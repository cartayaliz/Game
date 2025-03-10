using Logic;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Threading.Tasks;

SpectreConsoleVisual game = new SpectreConsoleVisual();

var gc = GameBuilder.CreateGame(game, game.players.ToArray(), game.N);

game.Play(gc);

public class SpectreConsoleVisual : IVisual
{
    public List<Player> players = new List<Player>();
    public Dictionary<string, string> mappedChar = new Dictionary<string, string>();

    public int N = 0;

    public string prompt = "Start Game";

    public SpectreConsoleVisual()
    {
        AnsiConsole.Clear();
        AnsiConsole.Markup("[underline red]Welcome !!! \n[/]");

        N = AnsiConsole.Prompt(new TextPrompt<int>("Board size? ").Validate(ValidateN));

        ValidationResult ValidateN(int n)
        {
            if (n <= 4) return ValidationResult.Error("[red]El tamaño del tablero debe ser al menos 5.[/]");
            return ValidationResult.Success();
        }

        ValidationResult ValidateNumberOfPlayer(int n)
        {
            if (n > 5) return ValidationResult.Error("[red]Too much.[/]");
            else if (n <= 0) return ValidationResult.Error("[red]Introduzca una cantidad válida de jugadores.[/]");
            return ValidationResult.Success();

        }

        var playerCount = AnsiConsole.Prompt(new TextPrompt<int>("Number of player?").Validate(ValidateNumberOfPlayer));

        List<string> usedType = new List<string>() { "A", "I", "E", "O", "F" };

        for (int i = 0; i < playerCount; i++)
        {
            var typePrompt = new TextPrompt<string>("What type?");
            foreach (var type in usedType)
            {
                typePrompt = typePrompt.AddChoice(type);
            }


            var typeOfPlayer = AnsiConsole.Prompt(typePrompt);

            usedType.Remove(typeOfPlayer);

            Player p = new PlayerIntelligent();
            if (typeOfPlayer == "A") p = new PlayerAstute();
            if (typeOfPlayer == "E") p = new PlayerExplorer();
            if (typeOfPlayer == "O") p = new PlayerObserver();
            if (typeOfPlayer == "F") p = new PlayerFast();

            var namePrompt = AnsiConsole.Prompt(new TextPrompt<string>("What is your name?"));
            p.user = namePrompt;

            players.Add(p);

        }
    }

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
        {"&", ":bicycle:"},
        {"$", ":vertical_traffic_light:"},
        {"?", ":bell:"},
        {"x", ":black_large_square:"},
        {"=", ":bridge_at_night:"},
        {"2s", ":oncoming_police_car:"},
        {"1m", ":camera: "},
        {"2v", ":eyes: "},
        {"+",  ":triangular_flag:"},
        {"%", ":magnifying_glass_tilted_left:"}
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
        prompt = $"{player.name} {player.user} {i}, {j} => {x}, {y} \n";
    }

    public void NextTurn(GameCenter gameCenter)
    {
        prompt = $"Next Turn: {gameCenter.GetCurrentPlayer().name}";
        Refresh(gameCenter);
    }

    public async void Winning(GameCenter gameCenter, Player player)
    {
        if (player.isWinner)
        {
            await MostrarAnimacionGanador(player);

            prompt = $"{player.name} {player.user} is the winner!!";
            Environment.Exit(0);
        }
        else
        {
            prompt = " Empate ";
            Refresh(gameCenter);
        }
    }

    private async Task MostrarAnimacionGanador(Player player)
    {
        // Mostrar la animación durante unos segundos
        for (int i = 0; i < 30; i++)
        {
            AnsiConsole.MarkupLine($"[italic red on white]¡Felicidades, {player.user} has ganado! :party_popper: :party_popper: :party_popper: [/]");
            Thread.Sleep(200); // Pausa corta
            AnsiConsole.Clear(); // Borra la línea anterior
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

        var tabletitle = new Table();
        tabletitle.AddColumn("[red] Emojis Mazer[/]");

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

            playerName += $" - {p.user}";
            int speed = p.speed;
            if (p.id == player.id) speed -= gameCenter.currentMove;
            table2.AddRow(p.id == player.id ? "[green] :check_mark: [/]" : "", playerName, p.visibility.ToString(), (speed).ToString(), AH, PH, OH);
        }

        AnsiConsole.Write(tabletitle);
        AnsiConsole.Write(table);
        AnsiConsole.Write(table2);
        AnsiConsole.Write(table3);
    }
    public void canNotMove(GameCenter gameCenter, Player player, int direction)
    {
        prompt = $"El user {player.name} no se puede mover para {direction}";
        Refresh(gameCenter);
    }

    public void canNotCallHability(GameCenter gameCenter, Player player, int index, Hability? hability)
    {
        prompt = $"El player {player.name} no se puede ejecutar la habilidad  {hability}";
        Refresh(gameCenter);
    }

    public void callHability(GameCenter gameCenter, Player player, int index, Hability? hability)
    {
        prompt = $"El player {player.name} ejecutó la habilidad  {hability}";
        Refresh(gameCenter);
    }

}
