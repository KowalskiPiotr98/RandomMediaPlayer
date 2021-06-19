using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RandomMediaPlayer.Core.Displayers.HistoryTracking
{
    public class HistoryTracker<T> where T : class
    {
        private readonly HashSet<T> history = new HashSet<T>();
        private bool isTracking = true;

        /// <summary>
        /// Indicates whether history is currently being tracket.
        /// </summary>
        public bool IsTracking
        {
            get => isTracking;
            set
            {
                if (!value)
                {
                    Clear();
                }
                isTracking = value;
            }
        }
        /// <summary>
        /// Read only list with display history
        /// </summary>
        public IReadOnlyCollection<T> SourceHistory => history.ToImmutableHashSet();
        /// <summary>
        /// Checks whether selected item was already displayed
        /// </summary>
        /// <param name="source">Source of the element</param>
        /// <returns>True if the item was already displayed, false otherwise</returns>
        public bool WasDisplayed(T source)
        {
            return history.Contains(source);
        }
        /// <summary>
        /// Tries to add item to history
        /// </summary>
        /// <param name="source">Item to add to history</param>
        /// <remarks>If tracking is disabled, true is always returned</remarks>
        /// <returns>True if the item was added correctly, false otherwise (for example if the item was already present in history)</returns>
        public bool AddToHistory(T source)
        {
            if (!IsTracking)
            {
                return true;
            }
            if (source is null)
            {
                return false;
            }
            return history.Add(source);
        }
        /// <summary>
        /// Clears display history
        /// </summary>
        public void Clear()
        {
            history.Clear();
        }
        /// <summary>
        /// Limits collection to only untracked items
        /// </summary>
        /// <param name="collection">Collection to limit</param>
        /// <param name="collectionSelector">Selector by which to select tracked object from collection</param>
        /// <returns>Collection without tracket items</returns>
        public IEnumerable<U> LimitCollectionToNotInHistory<U>(IEnumerable<U> collection, Func<U,T> collectionSelector)
        {
            return collection.Where(i => !history.Contains(collectionSelector(i)));
        }
    }
}
