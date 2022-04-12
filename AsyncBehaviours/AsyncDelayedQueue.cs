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

        public SemaphoreSlim oneAtATime = new SemaphoreSlim(1);
        private TimeSpan delay;
        private Action action;
        private Task lastRequest;
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
            lastRequest = ActuallyFire(cancellationToken);
            await lastRequest;
        }

        private async Task ActuallyFire(CancellationToken cancellationToken)
        {
            await oneAtATime.WaitAsync();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
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
            await (lastRequest ?? Task.CompletedTask);
        }
    }
}