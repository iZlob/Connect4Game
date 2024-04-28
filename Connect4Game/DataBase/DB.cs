using System.Data.Entity;

namespace Connect4Game.DataBase
{
    public class DB : DbContext
    {
        public DbSet<Players> players { get; set; }

        public DB() : base("Connect4")
        {

        }
    }
}
