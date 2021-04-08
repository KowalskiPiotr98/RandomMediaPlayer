using RandomMediaPlayer.Core.Displayables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RandomMediaPlayer.PhotoShower
{
    public class Photo : Displayable
    {
        private Image ImageDisplay => DisplayedOn as Image;

        public Photo(string source) : base(source)
        {

        }

        protected override void Display()
        {
            ImageDisplay.Source = new BitmapImage(new System.Uri(Source));
        }
        protected override void HideFromDisplay()
        {
            ImageDisplay.Source = null;
        }
        protected override bool IsControlValidForGivenType(UIElement control)
        {
            return control is Image;
        }
    }
}
