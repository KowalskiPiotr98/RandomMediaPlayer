using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using RandomMediaPlayer.Core.Displayers.HistoryTracking;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RandomMediaPlayer.Core.Displayers
{
    public class Displayer : IDisplayer, IHistoryTracking<string>
    {
        private IDisplayable currentDisplayable;
        protected IDirectoryPicker directoryPicker;
        protected Grid displayArea;
        protected UIElement displayElement;
        public HistoryTracker<string> HistoryTracker { get; }
        public string CurrentDisplayableName => currentDisplayable?.Source.Split('\\').Last();

        public Displayer(Grid displayArea, UIElement displayElement)
        {
            this.displayArea = displayArea;
            this.displayElement = displayElement;
            displayArea.Children.Add(displayElement);
            HistoryTracker = new HistoryTracker<string>();
        }

        public void Hide()
        {
            currentDisplayable?.Hide();
        }
        public void Next()
        {
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
