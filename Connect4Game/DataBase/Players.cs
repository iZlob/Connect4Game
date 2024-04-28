using System;

namespace Connect4Game.DataBase
{
    public class Players : IComparable<Players>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Score { get; set; }

        public Players()
        {
            Id = 0;
            Name = "";
            Password = "";
            Score = 0;
        }

        public int CompareTo(Players player)
        {
            if (player.Score < Score) return -1;
            else if (player.Score > Score) return 1;
            return 0;
        }
    }
}
