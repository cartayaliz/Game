using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace Logic
{

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
    public class CellVision : Cell
    {
        bool used = false;

        int delta = 1;

        public CellVision(int x, int y, int delta = 1, string id = "?") : base(x, y, id) { this.delta = delta; }

        public override void Move(Player p)
        {
            base.Move(p);
            if (!used)
            {
                p.visibility += delta;
                used = true;
            }
        }
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

        public bool[,] PlayerVisibility(Player p)
        {
            bool[,] result = new bool[n, n];
            if (p.remember)
            {
                foreach (var item in p.path)
                {
                    result[item.Item1, item.Item2] = true;
                }
            }
            var pos = playerPosition[p.id];
            int x = pos.Item1; int y = pos.Item2;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Math.Abs(x - i) <= p.visibility && Math.Abs(y - j) <= p.visibility)
                    {
                        result[i, j] = true;
                    }
                }
            }
            return result;

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