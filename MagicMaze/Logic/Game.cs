using System;
using System.Collections;

namespace Logic
{

    public class GameCenter
    {
        public Board board { get; set; }

        IVisual visual { get; set; }

        public List<Player> players { get; set; }

        int currentPlayer { get; set; }

        public int currentMove = 0;

        public bool finishedGame = false;

        public Player GetCurrentPlayer()
        {
            return players[currentPlayer];
        }

        public bool currentPlayerAction(int direction)
        {
            return doAction(GetCurrentPlayer(), direction);
        }

        public bool currentPlayerActivatehability(int hability)
        {
            return activateHability(GetCurrentPlayer(), hability);
        }

        public bool activateHability(Player p, int hability)
        {
            if (p.canDoAction(hability, this))
            {
                var h = p.doAction(hability, this);
                visual.callHability(this, p, hability, h);
                return true;
            }
            visual.canNotCallHability(this, p, hability, (hability >= 0 && hability < p.hability.Count) ? p.hability[hability] : null);
            return false;
        }

        public bool doAction(Player p, int direction)
        {
            var x = board.canMove(p, direction, this);
            if (x.HasValue)
            {
                p.starPlay();
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
            visual.canNotMove(this, p, direction);
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

}
