using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests
{
    [TestFixture]
    public class AsyncQueuedDelayTests:AsyncDelayTests
    {
        protected override IAsyncDelayed CreateInstance(TimeSpan cooldown, Action action)
        {
            return new AsyncDelayedQueue(cooldown, action);
        }
        
        
        [Test]
        public async Task Multiple_events_allFire()
        {
            List<Task> tasks = new List<Task>();
            var delay =  CreateInstance(T.ShortTime, Fire);
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(delay.Fire());
            }
            Assert.Less(Counter.Count,1000);
            await Task.WhenAll(tasks);
            Assert.AreEqual(1000, Counter.Count);
        }
          [Test]
        public async Task Stop_completes_all()
        {
            var adelay = CreateInstance(T.ShortTime, Counter.Increment);
            for (int i = 0; i < 100; i++)
            {
                await adelay.Fire();
            }
            await adelay.Stop();
            Assert.AreEqual(100,Counter.Count);
            Assert.False(adelay.isWaiting);
        }
    }
}