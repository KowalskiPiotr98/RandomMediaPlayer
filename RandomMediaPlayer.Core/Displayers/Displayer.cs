using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RandomMediaPlayer.HistoryTracking;
using RandomMediaPlayer.Storage.StorageHandlers;

namespace RandomMediaPlayer.Core.Displayers
{
    public abstract class Displayer : IDisplayer, IHistoryTracking
    {
        private IDisplayable currentDisplayable;
        protected IDirectoryPicker directoryPicker;
        protected Grid displayArea;
        protected UIElement displayElement;
        public IDirectoryPicker DirectoryPicker => directoryPicker;
        public HistoryTracker HistoryTracker { get; private set; }
        public string CurrentDisplayableName => currentDisplayable?.Source.Split('\\').Last();

        protected Displayer(Grid displayArea, UIElement displayElement)
        {
            this.displayArea = displayArea;
            this.displayElement = displayElement;
            displayArea.Children.Add(displayElement);
        }

        public void Hide()
        {
            currentDisplayable?.Hide();
        }
        public void Next()
        {
            HistoryTracker ??= new HistoryTracker(new HistoryStorageHandler(directoryPicker.BasePath));
            Hide();
            currentDisplayable = directoryPicker.GetRandomDisplayable(HistoryTracker);
            Refresh();
        }
        public void Refresh()
        {
            try
            {
                currentDisplayable?.DisplayOn(displayElement);
            }
            catch (System.IO.FileNotFoundException)
            {
                directoryPicker.ReadDisplayables();
                if (directoryPicker.IsEmpty)
                {
                    return;
                }
                Next();
            }
        }
        public void ReloadContent() => directoryPicker.ReadDisplayables();
        public void ClearDisplayArea()
        {
            displayArea.Children.Clear();
        }
    }
}
