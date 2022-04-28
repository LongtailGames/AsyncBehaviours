namespace com.longtailgames.asyncbehaviours
{
    public class AsyncDelayedQueue : IAsyncDelayed
    {
        /// <summary>
        /// Delays an action by the given timespan.
        /// The action is queued and all the events fire after their time has
        /// passed. <seealso cref="AsyncDelayedDropped"/>
        /// </summary>
        public bool isWaiting { get; private set; }

        public Task CurrentTask { get; private set; }

        public SemaphoreSlim oneAtATime = new SemaphoreSlim(1);
        private TimeSpan delay;
        private Action action;
        private readonly CancellationTokenSource stopSource;
        private readonly CancellationToken stopToken;

        public AsyncDelayedQueue(TimeSpan delay, Action action)
        {
            this.delay = delay;
            this.action = action;
            stopSource = new CancellationTokenSource();
            stopToken = stopSource.Token;
        }

        public async Task Fire(CancellationToken cancellationToken = default)
        {
            isWaiting = true;
            await Task.Delay(delay);
            CurrentTask = ActuallyFire(cancellationToken);
            await CurrentTask;
        }

        private async Task ActuallyFire(CancellationToken externaleCancellationToken)
        {
            try
            {
                await oneAtATime.WaitAsync(stopToken);
                externaleCancellationToken.ThrowIfCancellationRequested();
                stopToken.ThrowIfCancellationRequested();
                action.Invoke();
            }
            finally
            {
                oneAtATime.Release(1);
                isWaiting = false;
            }
        }

        public async Task Stop()
        {
            stopSource.Cancel();
            //special case of tasks not yet started but stopping.
            if (CurrentTask == null)
            {
                isWaiting = false;
            }
            await (CurrentTask ?? Task.CompletedTask);
        }
    }
}