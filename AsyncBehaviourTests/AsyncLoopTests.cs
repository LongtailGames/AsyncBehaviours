using System;
using System.Threading;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests
{
    [TestFixture]
    public class AsyncLoopTests
    {
        public Counter Counter = new Counter();
        private TimeSpan loopTime = TimeSpan.FromMilliseconds(10);

        [SetUp]
        public async Task ResetCounter()
        {
            Counter.Reset();
            await Task.Delay(0);
        }

        [Test]
        public async Task CreateALoop()
        {
            var aloop = new AsyncLoop(loopTime, null);
            Assert.NotNull(aloop);
        }

        [Test]
        public async Task LoopRuns()
        {
            var aloop = new AsyncLoop(loopTime, Counter.Increment);
            aloop.Start();
            Assert.IsTrue(aloop.Running);
            await Task.Delay(loopTime);
            Assert.Greater(Counter.Count, 0);
            await aloop.Stop();
        }

        [Test]
        public async Task StoppingLoop()
        {
            var aloop = new AsyncLoop(loopTime, Counter.Increment);
            aloop.Start();
            await
                Task.Delay(loopTime);
            await aloop.Stop();
            var countAtStop = Counter.Count;
            Assert.Greater(Counter.Count, 0);
            await Task.Delay(loopTime);
            await Task.Delay(loopTime);
            Assert.AreEqual(countAtStop, Counter.Count);
        }

        [Test]
        public async Task Cancel_stops_a_loop()
        {
            var aloop = new AsyncLoop(loopTime, Counter.Increment);
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken tkn = source.Token;
            source.CancelAfter(TimeSpan.FromMilliseconds(50));
            Assert.ThrowsAsync<OperationCanceledException>(async () => { await aloop.Start(tkn); });
        }

        [Test]
        public async Task StoppingLoopUsingDisposable()
        {
            var aloop = new AsyncLoop(loopTime, Counter.Increment);
            using (aloop)
            {
                aloop.Start();
                await
                    Task.Delay(loopTime);
                await
                    Task.Delay(loopTime);
            }

            var countAtStop = Counter.Count;
            Assert.Greater(Counter.Count, 0);
            await Task.Delay(loopTime);
            await Task.Delay(loopTime);
            Assert.AreEqual(countAtStop, Counter.Count);
        }
    }
}