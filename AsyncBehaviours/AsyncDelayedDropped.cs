using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBehaviours
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

        public async Task Fire(CancellationToken cancellationToken=default)
        {
            if (isWaiting)
            {
                return;
            }

            isWaiting = true;
            await Task.Delay(delay);
            action.Invoke();
            isWaiting = false;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}