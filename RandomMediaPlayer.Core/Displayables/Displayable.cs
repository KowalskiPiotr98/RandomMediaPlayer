using System.Windows;

namespace RandomMediaPlayer.Core.Displayables
{
    /// <summary>
    /// Object that can be displayed on the screen
    /// </summary>
    public abstract class Displayable : IDisplayable
    {
        public UIElement DisplayedOn { get; private set; }
        public string Source { get; private set; }

        protected Displayable(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new System.ArgumentNullException(nameof(source));
            }
            Source = source;
        }

        public void DisplayOn(UIElement control)
        {
            if (!IsControlValidForGivenType(control))
            {
                throw new System.ArgumentException("This control is invalid for given displayable object type.", nameof(control));
            }
            DisplayedOn = control;
            Display();
        }
        public void Hide()
        {
            if (DisplayedOn is null)
            {
                throw new System.InvalidOperationException("Cannot hide what wasn't yet displayed.");
            }
            HideFromDisplay();
            DisplayedOn = null;
        }

        protected abstract void Display();
        protected abstract void HideFromDisplay();
        protected abstract bool IsControlValidForGivenType(UIElement control);
    }
}
