using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using RandomMediaPlayer.Storage.StorageHandlers;

namespace RandomMediaPlayer.HistoryTracking
{
    public class HistoryTracker
    {
        private readonly HashSet<string> history;
        private bool isTracking = true;
        private readonly HistoryStorageHandler _storageHandler;
        public int SeenCount => history.Count;

        public HistoryTracker(HistoryStorageHandler storageHandler)
        {
            this._storageHandler = storageHandler;
            history = _storageHandler.GetAllHistory().ToHashSet();
        }

        public HistoryTracker(IEnumerable<string> storedHistory, HistoryStorageHandler storageHandler)
        {
            history = storedHistory.ToHashSet();
            this._storageHandler = storageHandler;
        }

        /// <summary>
        /// Indicates whether history is currently being tracked.
        /// </summary>
        public bool IsTracking
        {
            get => isTracking;
            set
            {
                if (!value)
                {
                    Clear().Wait();
                }
                isTracking = value;
            }
        }
        /// <summary>
        /// Read only list with display history
        /// </summary>
        public IReadOnlyCollection<string> SourceHistory => history.ToImmutableHashSet();
        /// <summary>
        /// Checks whether selected item was already displayed
        /// </summary>
        /// <param name="source">Source of the element</param>
        /// <returns>True if the item was already displayed, false otherwise</returns>
        public bool WasDisplayed(string source)
        {
            return history.Contains(source);
        }
        /// <summary>
        /// Tries to add item to history
        /// </summary>
        /// <param name="source">Item to add to history</param>
        /// <remarks>If tracking is disabled, true is always returned</remarks>
        /// <returns>True if the item was added correctly, false otherwise (for example if the item was already present in history)</returns>
        public async Task<bool> AddToHistory(string source)
        {
            if (!IsTracking)
            {
                return true;
            }
            if (source is null)
            {
                return false;
            }
            return history.Add(source) && await _storageHandler.TryAddToHistoryAsync(source);
        }
        /// <summary>
        /// Clears display history
        /// </summary>
        public async Task Clear()
        {
            history.Clear();
            await _storageHandler.ClearHistoryAsync();
        }
        /// <summary>
        /// Limits collection to only untracked items
        /// </summary>
        /// <param name="collection">Collection to limit</param>
        /// <param name="collectionSelector">Selector by which to select tracked object from collection</param>
        /// <returns>Collection without tracked items</returns>
        public IEnumerable<TU> LimitCollectionToNotInHistory<TU>(IEnumerable<TU> collection, Func<TU,string> collectionSelector)
        {
            return collection.Where(i => !history.Contains(collectionSelector(i)));
        }
    }
}
