namespace com.longtailgames.asyncbehaviours
{
    public interface IAsyncDelayed : IAsyncStop, IAsyncFire
    {
        bool isWaiting { get; }
        Task CurrentTask { get; }
    }
}