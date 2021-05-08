using RandomMediaPlayer.Core.Displayers;
using System.Windows.Controls;

namespace RandomMediaPlayer.MoviePlayer
{
    public class MovieDisplayer : Displayer
    {
        public MovieDisplayer(Grid displayArea, System.Uri directory) : base(displayArea, new Grid())
        {
            directoryPicker = new MovieDirectoryPicker(directory);
            var grid = displayElement as Grid;
            grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(10, System.Windows.GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(10, System.Windows.GridUnitType.Auto) });
            var media = new MediaElement { LoadedBehavior = MediaState.Manual };
            media.SetValue(Grid.RowProperty, 0);
            grid.Children.Add(media);
            var slider = new Slider();
            slider.SetValue(Grid.RowProperty, 1);
            grid.Children.Add(slider);
            Next();
        }
    }
}
