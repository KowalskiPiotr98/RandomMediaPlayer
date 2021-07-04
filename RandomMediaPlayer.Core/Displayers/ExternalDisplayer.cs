using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using System.Diagnostics;
using RandomMediaPlayer.HistoryTracking;
using RandomMediaPlayer.Storage.StorageHandlers;

namespace RandomMediaPlayer.Core.Displayers
{
    public class ExternalDisplayer : IDisplayer, IHistoryTracking
    {
        private IDisplayable currentDisplayable;
        private Process displayProcess;
        protected IDirectoryPicker directoryPicker;
        public IDirectoryPicker DirectoryPicker => directoryPicker;
        public HistoryTracker HistoryTracker { get; private set; }
        public string CurrentDisplayableName => null;

        public ExternalDisplayer(IDirectoryPicker directoryPicker)
        {
            this.directoryPicker = directoryPicker;
        }

        public void Hide()
        {
            displayProcess?.Kill();
        }
        public void Next()
        {
            HistoryTracker ??= new HistoryTracker(new HistoryStorageHandler(directoryPicker.BasePath));
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
