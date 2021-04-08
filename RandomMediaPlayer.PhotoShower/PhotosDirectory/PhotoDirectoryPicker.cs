using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayables;

namespace RandomMediaPlayer.PhotoShower.PhotosDirectory
{
    public class PhotoDirectoryPicker : DirectoryPicker
    {
        public PhotoDirectoryPicker(System.Uri directory) : base(directory)
        {
            AllowedExtentions = new string[] { "jpg", "png", "bmp" };
        }

        protected override IDisplayable CreateDisplayableFromLocalPath(string path) => new Photo(path);
    }
}
