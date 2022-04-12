using System;
using System.Threading;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests
{
    public abstract class AsyncDelayTests
    {
        public Counter Counter = new Counter();


        protected void Fire()
        {
            Counter.Increment();
        }

        protected abstract IAsyncDelayed CreateInstance(TimeSpan cooldown, Action action);

        [SetUp]
        public void Setup()
        {
            Counter.Reset();
        }


        [Test]
        public void AsyncDelayTestsSimplePasses()
        {
            var delay = CreateInstance(TimeSpan.Zero, null);
        }

        [Test]
        public async Task DelayInitiatially_not_fire()
        {
            var delay = CreateInstance(TimeSpan.Zero, Fire);
            await delay.Fire();
            Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public async Task ZeroDelay()
        {
            await Task.Delay(0);
            Assert.Pass();
        }

        [Test]
        public async Task AfterFire_Fired()
        {
            var delay = CreateInstance(TimeSpan.Zero, Fire);
            await delay.Fire();
            Assert.False(delay.isWaiting);
        }

        [Test]
        public void Initially_notWaiting()
        {
            var delay = CreateInstance(TimeSpan.Zero, Fire);
            Assert.False(delay.isWaiting);
        }


        [Test]
        public async Task WaitingTest()
        {
            var delay = CreateInstance(TimeSpan.FromMilliseconds(100), Fire);
            var t = delay.Fire();
            Assert.True(delay.isWaiting);
            await t;
            Assert.False(delay.isWaiting);
        }

        [Test]
        public async Task CancelTask()
        {
            var adelay = CreateInstance(T.LongTime, Counter.Increment);
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.CancelAfter(T.MediumTime);
            Assert.ThrowsAsync<OperationCanceledException>(async () => { await adelay.Fire(token); });
        }

        [Test]
        public async Task Stop_WithoutStarting_noError()
        {
            var adelay = CreateInstance(T.ShortTime, Counter.Increment);
            await adelay.Stop();
            Assert.False(adelay.isWaiting);
        }

        [Test]
        public async Task Stop_Awaits_completion()
        {
            var adelay = CreateInstance(T.ShortTime, Counter.Increment);
            await adelay.Fire();
            await adelay.Stop();
            Assert.False(adelay.isWaiting);
        }
      [Test]
        public async Task Halt_stop_immediately()
        {
            var delay =  CreateInstance(T.MediumTime, Fire);
            for (int i = 0; i < 4; i++)
            {
                delay.Fire();
            }

            var immediate = Counter.Count;
            await delay.Stop();
            var afterStop = Counter.Count;
            Assert.AreEqual(immediate,afterStop);
            Assert.False(delay.isWaiting);
        }
      
    }
}