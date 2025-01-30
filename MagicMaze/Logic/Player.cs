using System;
using System.Collections;

namespace Logic
{
    public class Player
    {
        public char name { get; set; }
        public int id { get; set; }
        public bool remember { get; set; }
        public bool isWinner { get; set; }
        public int speed { get; set; }
        public int visibility { get; set; } //Distancia Manhatan
        public int number_of_play { get; set; }
        public List<(int, int)> path { get; set; }
        public List<Hability> hability { get; set; }
        public Player(char name = 'a', int id = 1, int speed = 1, int visibility = 1, bool remember = false)
        {
            this.name = name;
            path = new List<(int, int)>();
            isWinner = false;
            this.remember = remember;
            this.id = id;
            this.speed = speed;
            this.visibility = visibility;
            hability = new List<Hability>();
            number_of_play = 0;
        }
        public override string ToString()
        {
            return $"[{id}] {name} {speed} {visibility}";
        }

        public void doAction()
        {

        }
        public void starPlay(Player player, GameCenter gameCenter)
        {
            number_of_play++;
        }
    }
    public class PlayerExplorador : Player
    {
        public PlayerExplorador(char name = 'E') : base(name, 1, 2, 2, false) { }
    }
    public class PlayerBuho : Player
    {
        public PlayerBuho(char name = 'B') : base(name, 2, 2, 1, true) { }
    }
    public class PlayerCorrecamino : Player
    {
        public PlayerCorrecamino(char name = 'C') : base(name, 3, 3, 1, false) { }
    }

    public class Hability
    {
        int lastActive = 0;
        int lastOff = 0;
        int TimeActive { get; set; }
        int TimeOff { get; set; }
        public virtual void Do(Player player, GameCenter gameCenter)
        {
            lastActive = player.number_of_play;
        }
        public virtual void Undo(Player player)
        {
            lastOff = player.number_of_play;
        }
        public virtual bool isActive(Player player)
        {
            if (lastOff + TimeOff <= player.number_of_play)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual void checkExpired(Player player)
        {
            if (lastActive > 0 && lastActive + TimeActive < player.number_of_play)
            {
                Undo(player);
            }
        }
    }

}