using RandomMediaPlayer.Core.Displayers;
using System;
using System.Windows.Threading;

namespace RandomMediaPlayer.Actions
{
    public class AutoAdvancer : IAutoAction, IDisposable
    {
        private IDisplayer displayer;
        private DispatcherTimer timer;
        private bool disposedValue;
        private int? prevValue;

        public AutoAdvancer(IDisplayer displayer, int seconds = 3)
        {
            Register(displayer, seconds);
        }

        public IAutoAction Register(IDisplayer displayer) => Register(displayer, prevValue ?? 3);
        public IAutoAction Register(IDisplayer displayer, int seconds)
        {
            prevValue = seconds;
            this.displayer = displayer;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(seconds)
            };
            timer.Tick+= MoveNext;
            timer.Start();
            return this;
        }
        public IAutoAction Unregister()
        {
            timer.Stop();
            return this;
        }
        private void MoveNext(object source, EventArgs eventArgs)
        {
            timer.Stop();
            displayer?.Next();
            timer.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Stop();
                    timer = null;
                    displayer = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
