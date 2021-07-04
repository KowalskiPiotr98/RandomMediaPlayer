using Microsoft.EntityFrameworkCore;

namespace RandomMediaPlayer.Storage.StorageHandlers
{
    public abstract class StorageHandler
    {
        protected readonly StorageContext _storageContext;

        protected StorageHandler(string dataSource = null)
        {
            if (dataSource is null)
            {
                _storageContext = new StorageContext();
            }
            else
            {
                var options = new DbContextOptionsBuilder();
                options.UseSqlite($"Data source={dataSource}");
                _storageContext = new StorageContext(options.Options);
            }
            EnsureUpToDate();
        }

        private void EnsureUpToDate()
        {
            _storageContext.Database.Migrate();
        }
    }
}