using RandomMediaPlayer.Core.Displayables;
using System.Collections.Generic;

namespace RandomMediaPlayer.Core.Directory
{
    public interface IDirectoryPicker
    {
        void ReadDisplayables();
        IEnumerable<IDisplayable> GetDisplayables();
        IDisplayable GetRandomDisplayable();
    }
}
