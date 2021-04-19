using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using System.Windows;
using System.Windows.Controls;

namespace RandomMediaPlayer.Core.Displayers
{
    public class Displayer : IDisplayer
    {
        private IDisplayable currentDisplayable;
        protected IDirectoryPicker directoryPicker;
        protected Grid displayArea;
        protected UIElement displayElement;

        public Displayer(Grid displayArea, UIElement displayElement)
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
            Hide();
            currentDisplayable = directoryPicker.GetRandomDisplayable();
            Refresh();
        }
        public void Refresh()
        {
            Resize();
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
        protected virtual void Resize() { }

    }
}
