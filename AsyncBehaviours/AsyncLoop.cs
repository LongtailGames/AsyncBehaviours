using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBehaviours
{
    public class AsyncLoop:IDisposable
    {
        private TimeSpan every;
        private Action action;
        public bool running { get; private set; }
        Task loop;

        public AsyncLoop(TimeSpan every, Action action)
        {
            this.every = every;
            this.action = action;
        }

        public async Task Start(CancellationToken token = default)
        {
            running = true;
            loop = Loop(token);
            await loop;
        }

        private async Task Loop(CancellationToken cancellationToken)
        {
            while (running)
            {
                cancellationToken.ThrowIfCancellationRequested();
                action?.Invoke();
                await Task.Delay(every);
            }
        }

        public async Task Stop()
        {
            running = false;
            await loop;
        }

        public void Dispose()
        {
            this?.Stop();
        }
    }
}