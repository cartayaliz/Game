using System;
using System.Collections;

namespace Logic
{
    public class Player
    {
        public char name { get; set; }
        public int id { get; set; }

        public bool isWinner { get; set; }
        public int speed { get; set; }

        public int visibility { get; set; } //Distancia Manhatan

        public List<(int, int)> path { get; set; }

        public Player(char name = 'a', int id = 1, int speed = 1, int visibility = 1)
        {
            this.name = name;
            path = new List<(int, int)>();
            isWinner = false;
            this.id = id;
            this.speed = speed;
            this.visibility = visibility;
        }

        public override string ToString()
        {
            return $"[{id}] {name} {speed} {visibility}";
        }

    }

    public interface IVisual
    {
        public void Init(GameCenter gameCenter);
        public void Play(GameCenter gameCenter);
        public void NextTurn(GameCenter gameCenter);

        public void Winning(GameCenter gameCenter, Player player);
        public void Refresh(GameCenter gameCenter);
        public void Move(GameCenter gameCenter, int i, int j, int x, int y, Player player);


    }
    class BasicConsoleVisual : IVisual
    {

        public void Init(GameCenter gameCenter)
        {
            System.Console.WriteLine("<init>");
            Console.WriteLine(gameCenter.ToString());
        }
        public void Play(GameCenter gc)
        {
            System.Console.WriteLine("play");
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
            Console.WriteLine(gameCenter.ToString());
        }

    }




    public class GameCenter
    {
        public Board board { get; set; }

        IVisual visual { get; set; }

        public List<Player> players { get; set; }

        int currentPlayer { get; set; }

        int currentMove = 0;

        public bool finishedGame = false;

        public Player GetCurrentPlayer()
        {
            return players[currentPlayer];
        }

        public bool currentPlayerAction(int direction)
        {
            return doAction(GetCurrentPlayer(), direction);
        }

        public bool doAction(Player p, int direction)
        {
            var x = board.canMove(p, direction, this);


            if (x.HasValue)
            {
                var pos = board.playerPosition[p.id];
                int i = pos.Item1;
                int j = pos.Item2;

                this.visual.Move(this, i, j, x.Value.Item1, x.Value.Item2, p);

                board.setPlayer(x.Value.Item1, x.Value.Item2, p);


                if (p.isWinner)
                {
                    finishedGame = true;
                    this.visual.Winning(this, p);
                    return true;
                }
                currentMove++;
                if (currentMove >= p.speed)
                {
                    nextTurn();
                }
                this.visual.Refresh(this);

                return true;
            }
            return false;

        }


        public void nextTurn()
        {
            if (finishedGame) return;
            currentPlayer++;
            currentMove = 0;
            if (currentPlayer == players.Count)
            {
                currentPlayer = 0;
            }
            this.visual.NextTurn(this);
        }

        public override string ToString()
        {
            string s = "";
            s += "Jugador actual: ";
            s += players[currentPlayer].ToString();
            s += "\n";
            s += board.ToString();
            return s;
        }

        public GameCenter(Board board, List<Player> players, IVisual visual)
        {
            this.board = board;
            this.currentPlayer = 0;
            this.players = players;
            this.visual = visual;
            this.visual.Init(this);
        }


    }

    public class Cell
    {
        int x { get; set; }
        int y { get; set; }

        public string id { get; set; }
        public Player? player { get; set; }

        public virtual bool canMove(Player p, GameCenter gc)
        {
            return player == null;
        }

        public virtual void RemovePlayer()
        {
            this.player = null;
        }


        public override string ToString()
        {
            if (this.player != null) return this.player.name.ToString();
            return id;
        }

        public Cell(int x, int y, string id = " ")
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }

        public virtual void Move(Player p)
        {
            p.path.Add((x, y));
            this.player = p;
        }
    }
    public class CellVision1 : Cell
    {
        public CellVision1(int x, int y) : base(x, y, "?") { }
    }
    public class CellSpeed1 : Cell
    {
        bool used = false;
        int delta = 1;
        public CellSpeed1(int x, int y, int delta = 1, string id = "&") : base(x, y, id) { this.delta = delta; }
        public override void Move(Player p)
        {
            base.Move(p);
            if (!used)
            {
                p.speed += delta;
                used = true;
            }
        }
    }

    public class CellBridge : Cell
    {
        bool used = false;
        public CellBridge(int x, int y, string id = "=") : base(x, y, id) { }

        public override bool canMove(Player p, GameCenter gc)
        {
            return !used;
        }
        public override void Move(Player p)
        {
            base.Move(p);
            if (!used)
            {
                used = true;
                this.id = "X";
            }
        }
    }
    public class CellSpeed2 : CellSpeed1
    {
        public CellSpeed2(int x, int y) : base(x, y, -1, "$") { }
    }
    public class CellWinn : Cell
    {
        public CellWinn(int x, int y) : base(x, y, "+")
        {

        }
        public override void Move(Player p)
        {
            base.Move(p);
            p.isWinner = true;
        }

    }

    public class CellObs : Cell
    {
        public CellObs(int x, int y) : base(x, y, "x") { }
        public override bool canMove(Player p, GameCenter gc)
        {
            return false;
        }
    }

    public class Board
    {
        public int n { get; set; }
        public Cell[,] matrix { get; set; }
        public Dictionary<int, (int, int)> playerPosition { get; set; }
        public int[] di = { -1, 0, 1, 0 };
        public int[] dj = { 0, 1, 0, -1 };
        public Board(int n)
        {
            this.n = n;
            matrix = new Cell[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = new Cell(i, j);
                }
            }
            playerPosition = new Dictionary<int, (int, int)>();

        }

        public override string ToString()
        {
            string s = "";
            string topSeparator = "";
            for (int i = 0; i < this.n + 2; i++)
                topSeparator += '_';
            string bottomSeparator = "";
            for (int i = 0; i < this.n + 2; i++)
                bottomSeparator += '-';
            for (int i = 0; i < this.n; i++)
            {
                string column = "|";
                for (int j = 0; j < this.n; j++)
                {
                    column += matrix[i, j].ToString();
                }
                column += "|\n";
                s += column;
            }
            return topSeparator + "\n" + s + bottomSeparator + "\n";
        }

        public (int, int)? canMove(Player p, int direction, GameCenter gc)
        {
            var pos = playerPosition[p.id];
            int ni = di[direction] + pos.Item1;
            int nj = dj[direction] + pos.Item2;
            if (IsValid(ni, nj) && matrix[ni, nj].canMove(p, gc))
            {
                return (ni, nj);
            }
            return null;
        }


        public bool IsValid(int i, int j)
        {
            return (i < n && j < n && i >= 0 && j >= 0);
        }
        public void setPlayer(int i, int j, Player p)
        {
            if (playerPosition.ContainsKey(p.id))
            {
                var pos = playerPosition[p.id];
                matrix[pos.Item1, pos.Item2].RemovePlayer();
            }
            playerPosition[p.id] = (i, j);
            matrix[i, j].Move(p);
        }
    }
}
