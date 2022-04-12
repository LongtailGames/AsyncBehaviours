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
        }

        public bool isWaiting { get; private set; }

        private TimeSpan delay;
        private Action action;
        private Task currentWait;

        public async Task Fire(CancellationToken cancellationToken=default)
        {
            if (isWaiting)
            {
                return;
            }

            isWaiting = true;
            currentWait =  Task.Delay(delay);
            await currentWait;
            action.Invoke();
            isWaiting = false;
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task Stop()
        {
            await (currentWait??Task.CompletedTask);
        }
    }
}