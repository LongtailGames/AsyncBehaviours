namespace com.longtailgames.asyncbehaviours;

public interface IAsyncStop
{
    /// <summary>
    /// Stops the current behaviour before the next action is fired.
    /// Once stopped the behaviour cannot be reused.
    /// </summary>
    /// <returns></returns>
    Task Stop();

}