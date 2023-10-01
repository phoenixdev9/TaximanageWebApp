using Microsoft.EntityFrameworkCore;

namespace api.Model
{
    public class Context : DbContext
    {
        public DbSet<Trip> Trips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(
                "Host=diplomski-b5520d2d.psql.b1t9.questdb.com;Port=30628;Database=qdb;Username=admin;Password=K5AAv5bItZ5CGY90;ServerCompatibilityMode=NoTypeLoading");
    }
}