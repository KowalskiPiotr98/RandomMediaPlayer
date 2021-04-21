using RandomMediaPlayer.Core.Displayers;

namespace RandomMediaPlayer.Actions
{
    public interface IAutoAction
    {
        IAutoAction Register(IDisplayer displayer);
        IAutoAction Unregister();
    }
}
