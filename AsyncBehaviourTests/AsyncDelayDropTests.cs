using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests
{
    [TestFixture]
    public class AsyncDelayDropTests:AsyncDelayTests
    {
        protected override IAsyncDelayed CreateInstance(TimeSpan cooldown, Action action)
        {
            return new AsyncDelayedDropped(cooldown, action);
        }
        
        
        [Test]
        public async Task Multiple_events_OneFires()
        {
                List<Task> tasks = new List<Task>();
            var delay =  CreateInstance(T.ShortTime, Fire);
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(delay.Fire());
            }
            await Task.WhenAll(tasks);
            Assert.AreEqual(1, Counter.Count);
        }
  
    }
}