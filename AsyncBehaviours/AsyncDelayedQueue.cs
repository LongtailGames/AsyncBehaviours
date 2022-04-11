using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBehaviours
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

        public AsyncDelayedQueue(TimeSpan delay, Action action)
        {
            this.delay = delay;
            this.action = action;
        }

        public async Task Fire(CancellationToken cancellationToken)
        {
            isWaiting = true;
            await Task.Delay(delay);
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
    }
}