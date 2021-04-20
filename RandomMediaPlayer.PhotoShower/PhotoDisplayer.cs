using RandomMediaPlayer.Core.Displayers;
using System.Windows.Controls;

namespace RandomMediaPlayer.PhotoShower
{
    public class PhotoDisplayer : Displayer
    {
        public PhotoDisplayer(Grid displayArea, System.Uri directory) : base(displayArea, new Image() )
        {
            directoryPicker = new PhotosDirectory.PhotoDirectoryPicker(directory);
            Next();
        }
    }
}
