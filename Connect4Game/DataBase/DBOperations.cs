using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Connect4Game.DataBase
{
    public class DBOperations
    {
        public DB db { get; set; }

        public DBOperations()
        {
            db = new DB();

            using (var connection = new SQLiteConnection("Data Source=Connect4DB.sqlite"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    string query = "CREATE TABLE IF NOT EXISTS Players " +
                        "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "Name TEXT, " +
                        "Password TEXT, " +
                        "Score INTEGER)";

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Players> GetAllPlayers()
        {
            var AllPlayers = new List<Players>();

            using (var db = new DB())
            {
                AllPlayers = db.players.ToList();
            }

            return AllPlayers;
        }

        public List<Players> GetChampions()
        {
            Players[] allPlayers;
            List<Players> champions = new List<Players>();

            using (var db = new DB())
            {
                allPlayers = db.players.ToArray();
            }

            Array.Sort(allPlayers);

            if (allPlayers.Length > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    champions.Add(allPlayers[i]);
                }
            }
            else
            {
                foreach (var player in allPlayers)
                {
                    champions.Add(player);
                }
            }

            return champions;
        }

        public bool CheckName(string name)
        {
            Players[] players;

            using (var db = new DB())
            {
                players = db.players.Where(x => x.Name == name).ToArray();
            }

            if (players.Length == 0)
            {
                return false;
            }

            return true;
        }

        public bool CheckNameAndPassword(string name, string pass)
        {
            Players[] players;

            using (var db = new DB())
            {
                players = db.players.Where(x => x.Name == name && x.Password == pass).ToArray();
            }

            if (players.Length == 0)
            {
                return false;
            }

            return true;
        }

        public void AddPlayer(Players player)
        {
            using (var db = new DB())
            {
                db.players.Add(player);
                db.SaveChanges();
            }
        }

        public void UpdateScore(string name, int score)
        {
            Players player;

            using (var db = new DB())
            {
                player = db.players.Where(x => x.Name == name).FirstOrDefault();
                player.Score = score;
                db.SaveChanges();
            }
        }

        public int GetPlayerScore(string name)
        {
            int score = 0;
            Players player;

            using (var db = new DB())
            {
                player = db.players.Where(x => x.Name == name).FirstOrDefault();
            }

            score = player.Score;

            return score;
        }
    }
}
