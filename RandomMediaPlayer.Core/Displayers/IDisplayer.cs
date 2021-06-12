namespace RandomMediaPlayer.Core.Displayers
{
    public interface IDisplayer
    {
        void Next();
        void Refresh();
        /// <summary>
        /// Reloads content available in selected source
        /// </summary>
        void ReloadContent();
        void Hide();
        string CurrentDisplayableName { get; }
    }
}
