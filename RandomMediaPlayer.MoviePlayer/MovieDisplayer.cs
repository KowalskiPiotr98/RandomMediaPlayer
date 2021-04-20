using RandomMediaPlayer.Core.Displayers;
using System.Windows.Controls;

namespace RandomMediaPlayer.MoviePlayer
{
    public class MovieDisplayer : Displayer
    {
        public MovieDisplayer(Grid displayArea, System.Uri directory) : base(displayArea, new MediaElement { LoadedBehavior = MediaState.Manual })
        {
            directoryPicker = new MovieDirectoryPicker(directory);
            Next();
        }
    }
}
