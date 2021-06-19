namespace RandomMediaPlayer.Core.Displayers.HistoryTracking
{
    /// <summary>
    /// Indicates the class implements <see cref="HistoryTracking.HistoryTracker"/> as a property.
    /// </summary>
    public interface IHistoryTracking<T> where T : class
    {
        HistoryTracker<T> HistoryTracker { get; }
    }
}
