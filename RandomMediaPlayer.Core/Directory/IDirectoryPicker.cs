using RandomMediaPlayer.Core.Displayables;
using System.Collections.Generic;

namespace RandomMediaPlayer.Core.Directory
{
    public interface IDirectoryPicker
    {
        bool IsEmpty { get; }
        void ReadDisplayables();
        IEnumerable<IDisplayable> GetDisplayables();
        IDisplayable GetRandomDisplayable();
        IDisplayable GetRandomDisplayable(RandomMediaPlayer.HistoryTracking.HistoryTracker<string> history);
    }
}
