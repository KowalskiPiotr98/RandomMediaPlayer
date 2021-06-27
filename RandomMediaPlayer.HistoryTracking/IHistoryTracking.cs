namespace RandomMediaPlayer.HistoryTracking
{
    /// <summary>
    /// Indicates the class implements <see cref="HistoryTracker"/> as a property.
    /// </summary>
    public interface IHistoryTracking
    {
        HistoryTracker HistoryTracker { get; }
    }
}
