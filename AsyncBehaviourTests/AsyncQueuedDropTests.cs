using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests
{
    [TestFixture]
    public class AsyncQueuedDropTests:AsyncDelayTests
    {
        protected override IAsyncDelayed CreateInstance(TimeSpan cooldown, Action action)
        {
            return new AsyncDelayedDropped(cooldown, action);
        }
        
        
        [Test]
        public async Task Multiple_events_allFire()
        {
                List<Task> tasks = new List<Task>();
            var delay =  CreateInstance(TimeSpan.FromMilliseconds(100), Fire);
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(delay.Fire());
            }
            await Task.WhenAll(tasks);
            Assert.AreEqual(1, Counter.Count);
        }
    }
}