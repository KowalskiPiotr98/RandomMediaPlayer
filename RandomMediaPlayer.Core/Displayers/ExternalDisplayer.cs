using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using System.Diagnostics;

namespace RandomMediaPlayer.Core.Displayers
{
    public class ExternalDisplayer : IDisplayer
    {
        private IDisplayable currentDisplayable;
        private Process displayProcess;
        protected IDirectoryPicker directoryPicker;

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
            Hide();
            currentDisplayable = directoryPicker.GetRandomDisplayable();
            displayProcess = Display();
        }
        public void Refresh()
        {
            Hide();
            Display();
        }

        protected Process Display()
        {
            var pi = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = currentDisplayable.Source
            };
            return Process.Start(pi);
        }
    }
}
