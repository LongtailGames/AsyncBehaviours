namespace com.longtailgames.asyncbehaviours;

public interface IAsyncFire
{
    /// <summary>
    /// Attempts to fire the action
    /// </summary>
    /// <param name="token">Optional cancellation token.</param>
    /// <returns></returns>
    Task Fire(CancellationToken token = default);
}