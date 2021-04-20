using RandomMediaPlayer.Core.Displayables;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RandomMediaPlayer.MoviePlayer
{
    public class Movie : Displayable
    {
        private MediaElement MovieDisplay => DisplayedOn as MediaElement;
        private readonly Uri sourceUri;
        public Movie(string source) : base(source)
        {
            sourceUri = new Uri(Source);
        }

        protected override void Display()
        {
            MovieDisplay.Source = sourceUri;
            MovieDisplay.Play();
        }
        protected override void HideFromDisplay()
        {
            MovieDisplay.Stop();
            MovieDisplay.Source = null;
        }
        protected override bool IsControlValidForGivenType(UIElement control) => control is MediaElement;
    }
}
