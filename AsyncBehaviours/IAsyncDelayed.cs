namespace com.longtailgames.asyncbehaviours
{
    public interface IAsyncDelayed
    {
        bool isWaiting { get; }
        Task Fire(CancellationToken cancellationToken=default);
    }
}