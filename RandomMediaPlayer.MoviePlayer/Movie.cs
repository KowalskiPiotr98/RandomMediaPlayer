using RandomMediaPlayer.Core.Displayables;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RandomMediaPlayer.MoviePlayer
{
    public class Movie : Displayable
    {
        private MediaElement MovieDisplay => (DisplayedOn as Grid).Children[0] as MediaElement;
        private Slider TimeSlider => (DisplayedOn as Grid).Children[1] as Slider;
        private readonly Uri sourceUri;
        private bool paused = true;
        private readonly DispatcherTimer positionTimer;
        private bool ignoreSliderUpdates;
        public Movie(string source) : base(source)
        {
            sourceUri = new Uri(Source);
            positionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1)
            };
            positionTimer.Tick += PositionTimer_Tick;
        }

        protected override void Display()
        {
            MovieDisplay.Source = sourceUri;
            TimeSlider.Minimum = 0;
            TimeSlider.LargeChange = 15;
            MovieDisplay.MediaOpened += SetSliderMax;
            MovieDisplay.MouseDown += PausePlay;
            TimeSlider.PreviewMouseUp += TimeSliderSeek;
            TimeSlider.PreviewMouseDown += IgnoreUpdates;
            MovieDisplay.Play();
            positionTimer.Start();
            paused = false;
        }

        private void IgnoreUpdates(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreSliderUpdates = true;
        }
        private void PositionTimer_Tick(object sender, EventArgs e) => UpdateSlider();

        private void PausePlay(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DisplayedOn is null || e.ChangedButton != System.Windows.Input.MouseButton.Left)
            {
                return;
            }
            if (paused)
            {
                MovieDisplay.Play();
                positionTimer.Start();
                paused = false;
            }
            else
            {
                MovieDisplay.Pause();
                positionTimer.Stop();
                paused = true;
            }
        }

        private void TimeSliderSeek(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ignoreSliderUpdates = false;
            if (DisplayedOn is null)
            {
                return;
            }
            var slider = sender as Slider;
            MovieDisplay.Stop();
            MovieDisplay.Position = TimeSpan.FromSeconds(slider.Value);
            MovieDisplay.Play();
        }

        private void UpdateSlider()
        {
            if (ignoreSliderUpdates)
            {
                return;
            }
            TimeSlider.Value = MovieDisplay.Position.TotalSeconds;
        }

        private void SetSliderMax(object sender, RoutedEventArgs e)
        {
            if (DisplayedOn is null)
            {
                return;
            }
            if (MovieDisplay.NaturalDuration.HasTimeSpan)
            {
                TimeSlider.Maximum = MovieDisplay.NaturalDuration.TimeSpan.TotalSeconds;
                MovieDisplay.MediaOpened -= SetSliderMax;
                positionTimer.Interval = TimeSpan.FromSeconds(MovieDisplay.NaturalDuration.TimeSpan.TotalSeconds / 3000);
            }
        }

        protected override void HideFromDisplay()
        {
            MovieDisplay.Stop();
            MovieDisplay.MediaOpened -= SetSliderMax;
            MovieDisplay.MouseDown -= PausePlay;
            TimeSlider.PreviewMouseUp -= TimeSliderSeek;
            positionTimer.Stop();
            MovieDisplay.Source = null;
        }
        protected override bool IsControlValidForGivenType(UIElement control) => control is Grid && (control as Grid).Children[0] is MediaElement && (control as Grid).Children[1] is Slider;
    }
}
