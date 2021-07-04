using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;

namespace RandomMediaPlayer.MoviePlayer
{
    public class MovieDirectoryPicker : DirectoryPicker
    {
        public MovieDirectoryPicker(System.Uri directory) : base(directory)
        {
            AllowedExtensions = new string[] { "mp4", "ts", "webm" };
        }
        protected override IDisplayable CreateDisplayableFromLocalPath(string path) => new Movie(path);
    }
}
