namespace com.longtailgames.asyncbehaviours
{
    public class AsyncDelayedDropped : IAsyncDelayed
    {
        /// <summary>
        /// Delays an action by the given timespan.
        /// Other actions attempted while waiting are dropped. <seealso cref="AsyncDelayedDropped"/>
        /// </summary>
        public AsyncDelayedDropped(TimeSpan delay, Action action)
        {
            this.delay = delay;
            this.action = action;
            stopSource = new CancellationTokenSource();
            stopToken = stopSource.Token;
        }

        public bool isWaiting { get; private set; }

        private TimeSpan delay;
        private Action action;
        private Task currentWait;
        private bool stopped;
        private readonly CancellationTokenSource stopSource;
        private readonly CancellationToken stopToken;

        public async Task Fire(CancellationToken cancellationToken = default)
        {
            if (isWaiting)
            {
                return;
            }

            try
            {
                isWaiting = true;
                currentWait = Task.Delay(delay);
                await currentWait;
                cancellationToken.ThrowIfCancellationRequested();
                stopToken.ThrowIfCancellationRequested();
                if (stopped)
                {
                    return;
                }
                action.Invoke();
            }
            finally
            {
                isWaiting = false;
            }
        }

        public async Task Stop()
        {
            stopSource.Cancel();
            await (currentWait ?? Task.CompletedTask);
        }
    }
}