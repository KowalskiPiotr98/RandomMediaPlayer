using Microsoft.EntityFrameworkCore;
using RandomMediaPlayer.Storage.Models;

namespace RandomMediaPlayer.Storage
{
    public class StorageContext : DbContext
    {
        public StorageContext() : this(new DbContextOptionsBuilder().UseSqlite("Data source=data.db").Options)
        {

        }
        public StorageContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UriHistory>().HasKey(e => new {e.BasePath, e.EntityName});
        }

        public DbSet<UriHistory> UriHistory { get; set; }
    }
}