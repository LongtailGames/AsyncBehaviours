namespace com.longtailgames.asyncbehaviours
{
    /// <summary>
    /// Execute an action every interval.
    /// </summary>
    public class AsyncLoop : IDisposable
    {
        private readonly TimeSpan every;
        private readonly Action action;
        public bool Running { get; private set; }
        Task loop;

        public AsyncLoop(TimeSpan every, Action action)
        {
            this.every = every;
            this.action = action;
        }

        /// <summary>
        /// Start the loop.
        /// </summary>
        /// <param name="cancellationToken">An optional cancellation cancellationToken</param>
        public async Task Start(CancellationToken cancellationToken = default)
        {
            Running = true;
            loop = Loop(cancellationToken);
            await loop;
        }

        private async Task Loop(CancellationToken cancellationToken)
        {
            while (Running)
            {
                cancellationToken.ThrowIfCancellationRequested();
                action?.Invoke();
                await Task.Delay(every);
            }
        }

        /// <summary>
        /// Stops the looping behaviour.
        /// Equivalent to <seealso cref="Dispose"/>
        /// </summary>
        public async Task Stop()
        {
            Running = false;
            await loop;
        }

        /// <summary>
        /// Stops the looping behaviour.
        /// Equivalent to <seealso cref="Stop"/>
        /// </summary>
        public void Dispose()
        {
            this?.Stop();
        }
    }
}