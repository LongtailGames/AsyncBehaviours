namespace com.longtailgames.asyncbehaviours;

public class AsyncActionExecuter
{
    private Func<Task> asyncAction;

    public async Task ExecuteAsync()
    {
        await asyncAction();
    }

    public AsyncActionExecuter(Func<Task> Asyncmethod)
    {
        this.asyncAction = Asyncmethod;
    }
    public AsyncActionExecuter(Action method)
    {
            
        asyncAction = () =>
        {
            method.Invoke();
            return Task.CompletedTask;
        };
    }
}