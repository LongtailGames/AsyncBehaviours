using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests
{
    [TestFixture]
    public class AsyncCooldownTests
    {
        public Counter Counter = new Counter();
        public Counter DropCounter = new Counter();


        [SetUp]
        public async Task ResetCounter()
        {
            Counter.Reset();
            DropCounter.Reset();
            await Task.Delay(0);
        }

        [Test]
        public async Task InitiallyNotCooled()
        {
            var cool = new AsyncCooldown(TimeSpan.Zero, null);
            Assert.False(cool.IsCooldown);
        }

        [Test]
        public async Task FireResultsCooldown()
        {
            var acool = new AsyncCooldown(T.ShortTime, Counter.Increment);
            acool.Fire();
            Assert.True(acool.IsCooldown);
        }

        [Test]
        public async Task AwaitingFireWaitsForCooldown()
        {
            var acool = new AsyncCooldown(T.ShortTime, Counter.Increment);
            await acool.Fire();
            Assert.False(acool.IsCooldown);
        }

        [Test]
        public async Task FirstFireWorks()
        {
            var acool = new AsyncCooldown(T.ShortTime, Counter.Increment);
            await acool.Fire();
            Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public async Task AwaitingFire()
        {
            var acool = new AsyncCooldown(T.ShortTime, Counter.Increment);
            for (int i = 0; i < 10; i++)
            {
                await acool.Fire();
            }

            Assert.AreEqual(10, Counter.Count);
        }

        [Test]
        public async Task FireMultipleResultsInCancelled()
        {
            var acool = new AsyncCooldown(T.ShortTime, Counter.Increment, DropCounter.Increment);
            for (int i = 0; i < 10; i++)
            {
                acool.Fire();
            }

            Assert.Greater(DropCounter.Count, 1);
        }

        [Test]
        public async Task MulitFire_droppedPlusFired_equalTotal()
        {
            var acool = new AsyncCooldown(T.ShortTime, Counter.Increment, DropCounter.Increment);
            for (int i = 0; i < 10; i++)
            {
                acool.Fire();
            }

            var total = Counter.Count + DropCounter.Count
                ;
            Assert.AreNotEqual(0, DropCounter);
            Assert.AreNotEqual(0, Counter);
            Assert.AreEqual(10, total);
        }

        [Test]
        public async Task CancelTask()
        {
            var acool = new AsyncCooldown(T.LongTime, Counter.Increment, DropCounter.Increment);

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.CancelAfter(T.MediumTime);
            Assert.ThrowsAsync<OperationCanceledException>(async () => { await acool.Fire(token); });
        }
    }
}