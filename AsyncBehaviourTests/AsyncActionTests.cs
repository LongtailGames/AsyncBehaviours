using System;
using System.Threading.Tasks;
using com.longtailgames.asyncbehaviours;
using NUnit.Framework;

namespace AsyncBehaviourTests;

[TestFixture]
public class AsyncActionTests
{
    private AsyncActionExecuter executer;
    public Counter Counter = new Counter();
    private TimeSpan loopTime = TimeSpan.FromMilliseconds(10);

    [SetUp]
    public async Task ResetCounter()
    {
        Counter.Reset();
        await Task.Delay(0);
    }

    [Test]
    public async Task ExecuteOnce()
    {
        executer = new AsyncActionExecuter(Counter.Increment);
        await executer.ExecuteAsync();
        Assert.AreEqual(1, Counter.Count);
    }

    [Test]
    public async Task ExecuteOnceAsync()
    {   executer = new AsyncActionExecuter(Counter.IncrementAsync);
        await executer.ExecuteAsync();
        Assert.AreEqual(1, Counter.Count);
        
    }
}