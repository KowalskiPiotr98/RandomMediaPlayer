using RandomMediaPlayer.Core.Displayers;
using System.Windows.Controls;

namespace RandomMediaPlayer.PhotoShower
{
    public class PhotoDisplayer : Displayer
    {
        private Image DisplayImage => displayElement as Image;
        public PhotoDisplayer(Grid displayArea, System.Uri directory) : base(displayArea, new Image() )
        {
            directoryPicker = new PhotosDirectory.PhotoDirectoryPicker(directory);
            Next();
        }

        protected override void Resize()
        {
            DisplayImage.Width = displayArea.Width;
        }
    }
}
