namespace com.longtailgames.asyncbehaviours
{
    /// <summary>
    /// A cooldown based behaviour. Useful for events that need recharging.
    /// </summary>
    public class AsyncCooldown : IAsyncStop, IAsyncFire
    {
        public bool IsCooldown { get; private set; }
        private readonly TimeSpan cooldown;
        private readonly Action action;
        private readonly Action cancelled;
        private Task currentJob;

        public AsyncCooldown(TimeSpan cooldown, Action action)
        {
            this.cooldown = cooldown;
            this.action = action;
        }

        public AsyncCooldown(TimeSpan cooldown, Action action, Action cancelled)
        {
            this.cancelled = cancelled;
            this.cooldown = cooldown;
            this.action = action;
        }

        /// <summary>
        /// If not in cooldown will fire the action and start cooldown.
        ///
        /// If in cooldown fire cancelled action.
        /// 
        /// </summary>
        public async Task Fire(CancellationToken token = default)
        {
            if (IsCooldown)
            {
                cancelled?.Invoke();
                return;
            }

            IsCooldown = true;
            action.Invoke();
            currentJob = Task.Delay(cooldown);
            await currentJob;
            IsCooldown = false;
            token.ThrowIfCancellationRequested();
        }

        public async Task Stop()
        {
            await (currentJob ?? Task.CompletedTask);
        }
    }
}