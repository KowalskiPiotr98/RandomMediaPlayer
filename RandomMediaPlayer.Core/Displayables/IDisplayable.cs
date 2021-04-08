namespace RandomMediaPlayer.Core.Displayables
{
    /// <summary>
    /// An element that can be displayed on the UI
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// Indicates on which control is the element displayed
        /// </summary>
        System.Windows.UIElement DisplayedOn { get; }
        /// <summary>
        /// Source of the element
        /// </summary>
        string Source { get; }
        /// <summary>
        /// Displays the element on given control
        /// </summary>
        /// <param name="control">The control to display the element at</param>
        void DisplayOn(System.Windows.UIElement control);
        /// <summary>
        /// Hides the element from the <see cref="DisplayedOn"/> control
        /// </summary>
        void Hide();
    }
}
