using System;
using System.Collections;

namespace Logic
{
    public class Player
    {
        public char name { get; set; }

        public string user = "";

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

        public Hability? doAction(int i, GameCenter gc)
        {
            if (canDoAction(i, gc))
            {

                hability[i].Do(this, gc);
                return hability[i];
            }
            return null;
        }

        public bool canDoAction(int i, GameCenter gc)
        {
            if (i >= 0 && i < hability.Count && hability[i].canBeActive(this))
            {
                return true;
            }
            return false;
        }


        public void starPlay()
        {
            number_of_play++;
            for (int i = 0; i < hability.Count; i++)
            {
                var item = hability[i];
                item.checkExpired(this);
            }
        }
    }
    public class PlayerExplorer : Player
    {
        public PlayerExplorer(char name = 'E') : base(name, 1, 4, 2, false) { this.hability.Add(new HabilitySpeed()); }
    }
    public class PlayerIntelligent : Player
    {
        public PlayerIntelligent(char name = 'I') : base(name, 2, 4, 1, true)
        {
            this.hability.Add(new HabilityVisibility());
        }
    }
    public class PlayerFast : Player
    {
        public PlayerFast(char name = 'F') : base(name, 3, 6, 1, false) { this.hability.Add(new HabilityMemory()); }
    }
    public class PlayerAstute : Player
    {
        public PlayerAstute(char name = 'A') : base(name, 4, 4, 2, false)
        {
            this.hability.Add(new HabilityMemory());
        }
    }
    public class PlayerObserver : Player
    {
        public PlayerObserver(char name = 'O') : base(name, 5, 4, 1, true)
        {
            this.hability.Add(new HabilityVisibility());
        }
    }
    public class Hability
    {
        protected int lastActive = 0;
        protected int lastOff = 0;

        public bool active = false;
        protected int TimeActive { get; set; }
        protected int TimeOff { get; set; }
        public virtual void Do(Player player, GameCenter gameCenter)
        {
            lastActive = player.number_of_play;
            active = true;
        }
        public virtual void Undo(Player player)
        {
            active = false;
            lastOff = player.number_of_play;
        }
        public virtual bool canBeActive(Player player)
        {
            if (!active && lastOff + TimeOff <= player.number_of_play)
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
            if (lastActive > 0 && active && lastActive + TimeActive < player.number_of_play)
            {
                Undo(player);
            }
        }
        public Hability(int TimeActive = 0, int TimeOff = 0)
        {
            this.TimeActive = TimeActive;
            this.TimeOff = TimeOff;
        }
    }
    public class HabilityVisibility : Hability
    {
        public HabilityVisibility() : base(2, 3)
        {
        }
        public override void Do(Player player, GameCenter gameCenter)
        {
            player.visibility += 2;
            base.Do(player, gameCenter);

        }
        public override void Undo(Player player)
        {
            player.visibility -= 2;
            base.Undo(player);
        }

        public override string ToString()
        {
            return "2v";
        }
    }
    public class HabilityMemory : Hability
    {
        public HabilityMemory() : base(10, 3) { }
        public override void Do(Player player, GameCenter gameCenter)
        {
            player.remember = true;
            base.Do(player, gameCenter);
        }
        public override void Undo(Player player)
        {
            player.remember = false;
            base.Undo(player);
        }
        public override string ToString()
        {
            return "1m";
        }
    }
    public class HabilitySpeed : Hability
    {
        public HabilitySpeed() : base(4, 5) { }
        public override void Do(Player player, GameCenter gameCenter)
        {
            player.speed += 2;
            base.Do(player, gameCenter);
        }
        public override void Undo(Player player)
        {
            player.speed -= 2;
            base.Undo(player);

        }
        public override string ToString()
        {
            return "2s";
        }
    }
}
