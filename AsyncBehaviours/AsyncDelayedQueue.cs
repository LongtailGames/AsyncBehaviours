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

        public AsyncDelayedQueue(TimeSpan delay, Action action)
        {
            this.delay = delay;
            this.action = action;
        }

        public async Task Fire(CancellationToken cancellationToken=default)
        {
            isWaiting = true;
            await Task.Delay(delay);
            lastRequest =  ActuallyFire(cancellationToken);
            await lastRequest;
        }

        private async Task ActuallyFire(CancellationToken cancellationToken)
        {
            await oneAtATime.WaitAsync();
            try
            {
                action.Invoke();
            }
            finally
            {
                oneAtATime.Release(1);
                isWaiting = false;
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public async Task Stop()
        {
            await (lastRequest ?? Task.CompletedTask);
        }
    }
}