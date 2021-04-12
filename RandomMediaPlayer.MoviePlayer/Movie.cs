using RandomMediaPlayer.Core.Displayables;
using System.Windows;

namespace RandomMediaPlayer.MoviePlayer
{
    public class Movie : Displayable
    {
        public Movie(string source) : base(source)
        {

        }

        protected override void Display() => throw new System.NotImplementedException();
        protected override void HideFromDisplay() => throw new System.NotImplementedException();
        protected override bool IsControlValidForGivenType(UIElement control) => throw new System.NotImplementedException();
    }
}
