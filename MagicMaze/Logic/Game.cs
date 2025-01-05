using System;
using System.Collections;

namespace Logic
{
    class Player
    {
        public char name { get; set; }
        public int id { get; set; }

        public bool isWinner { get; set; }
        public int speed { get; set; }

        public List<(int, int)> path { get; set; }

        public Player(char name = 'a', int id = 1, int speed = 1)
        {
            this.name = name;
            path = new List<(int, int)>();
            isWinner = false;
            this.id = id;
            this.speed = speed;
        }

        public override string ToString()
        {
            return $"[{id}] {name} {speed}";
        }

    }

    class GameCenter
    {
        public Board board { get; set; }
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
                board.setPlayer(x.Value.Item1, x.Value.Item2, p);

                if (p.isWinner)
                {
                    finishedGame = true;
                }
                currentMove++;
                if (currentMove >= p.speed)
                {
                    nextTurn();
                }
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

        public GameCenter(Board board, List<Player> players)
        {
            this.board = board;
            this.currentPlayer = 0;
            this.players = players;
        }


    }


    class Cell
    {
        int x { get; set; }
        int y { get; set; }

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
            return " ";
        }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Move(Player p)
        {
            p.path.Add((x, y));
            this.player = p;
        }
    }
    class CellSpeed : Cell
    {
        bool used = false;
        public CellSpeed(int x, int y) : base(x, y) { }
        public override string ToString()
        {
            return "&";
        }
        public override void Move(Player p)
        {
            base.Move(p);
            if (!used)
            {
                p.speed++;
                used = true;
            }
        }
    }
    class CellWinn : Cell
    {
        public CellWinn(int x, int y) : base(x, y)
        {

        }
        public override string ToString()
        {
            return "+";
        }
        public override void Move(Player p)
        {
            base.Move(p);
            p.isWinner = true;
        }

    }

    class CellObs : Cell
    {
        public CellObs(int x, int y) : base(x, y) { }
        public override bool canMove(Player p, GameCenter gc)
        {
            return false;
        }
        public override string ToString()
        {
            return "X";
        }
    }

    class Board
    {
        public int n { get; set; }
        public Cell[,] matrix { get; set; }
        Dictionary<int, (int, int)> playerPosition { get; set; }
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
