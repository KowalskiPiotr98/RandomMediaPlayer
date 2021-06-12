using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using RandomMediaPlayer.Core.Displayers.HistoryTracking;
using System.Diagnostics;

namespace RandomMediaPlayer.Core.Displayers
{
    public class ExternalDisplayer : IDisplayer, IHistoryTracking<string>
    {
        private IDisplayable currentDisplayable;
        private Process displayProcess;
        protected IDirectoryPicker directoryPicker;
        public HistoryTracker<string> HistoryTracker { get; }

        public ExternalDisplayer(IDirectoryPicker directoryPicker)
        {
            this.directoryPicker = directoryPicker;
            HistoryTracker = new HistoryTracker<string>();
        }

        public void Hide()
        {
            displayProcess?.Kill();
        }
        public void Next()
        {
            Hide();
            currentDisplayable = directoryPicker.GetRandomDisplayable(HistoryTracker);
            displayProcess = Display();
        }
        public void Refresh()
        {
            Hide();
            Display();
        }

        protected Process Display()
        {
            if (currentDisplayable is null)
            {
                return null;
            }
            var pi = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = currentDisplayable.Source
            };
            return Process.Start(pi);
        }
        public void ReloadContent() => directoryPicker.ReadDisplayables();

    }
}
