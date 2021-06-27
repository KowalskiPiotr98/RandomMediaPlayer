using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using RandomMediaPlayer.Storage.Models;

namespace RandomMediaPlayer.Storage.StorageHandlers
{
    public class HistoryStorageHandler : StorageHandler
    {
        private readonly string _basePath;

        public HistoryStorageHandler(string basePath, string dataSource = null) : base(dataSource)
        {
            _basePath = basePath;
        }

        /// <summary>
        /// Gets all history for specifies base path
        /// </summary>
        /// <returns>All entity names recorded for given base path</returns>
        public IEnumerable<string> GetAllHistory()
        {
            return GetAllInBasePath().CreateEnumerableString();
        }

        /// <summary>
        /// Removes all items in given base path
        /// </summary>
        public async Task ClearHistoryAsync()
        {
            _storageContext.UriHistory.RemoveRange(GetAllInBasePath());
            await _storageContext.SaveChangesAsync();
        }

        /// <summary>
        /// Tries to add an item to history
        /// </summary>
        /// <param name="entityName">Entity name to add to history</param>
        /// <returns><c>true</c> if item was added, <c>false</c> otherwise</returns>
        public async Task<bool> TryAddToHistoryAsync(string entityName)
        {
            try
            {
                _storageContext.UriHistory.Add(new UriHistory(_basePath, entityName));
                await _storageContext.SaveChangesAsync();
                return true;
            }
            catch (DbException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all items in history for given base path and replaces them with provided values
        /// </summary>
        /// <param name="newEntityNames">New history to save in database</param>
        public async Task RecreateHistoryAsync(IEnumerable<string> newEntityNames)
        {
            await ClearHistoryAsync();
            foreach (var newEntityName in newEntityNames)
            {
                _storageContext.UriHistory.Add(new UriHistory(_basePath, newEntityName));
            }

            await _storageContext.SaveChangesAsync();
        }

        private IQueryable<UriHistory> GetAllInBasePath()
        {
            return _storageContext.UriHistory.Where(h => h.BasePath == _basePath);
        }

    }

    internal static class HistoryStorageQueryableExtension
    {
        public static IEnumerable<string> CreateEnumerableString(this IQueryable<UriHistory> history)
        {
            return history.Select(h => h.BasePath + h.EntityName).AsEnumerable();
        }
    }
}