using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;
using System.Windows;
using System.Windows.Controls;

namespace RandomMediaPlayer.Core.Displayers
{
    public abstract class Displayer : IDisplayer
    {
        private IDisplayable currentDisplayable;
        protected IDirectoryPicker directoryPicker;
        protected Grid displayArea;
        protected UIElement displayElement;

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
            currentDisplayable = directoryPicker.GetRandomDisplayable();
            Refresh();
        }
        public void Refresh()
        {
            Resize();
            currentDisplayable.DisplayOn(displayElement);
        }
        protected virtual void Resize() { }
    }
}
