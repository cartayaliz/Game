
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

    public class BasicConsoleVisual : IVisual
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
            Refresh(gameCenter);
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
        private async Task MostrarAnimacionGanador(Player player)
        {
            Console.WriteLine($"¡Felicidades, {player.user} has ganado![/]");
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

}


