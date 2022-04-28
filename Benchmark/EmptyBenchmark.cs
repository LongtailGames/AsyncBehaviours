using AsyncBehaviourTests;
using BenchmarkDotNet.Attributes;
using com.longtailgames.asyncbehaviours;

namespace Benchmark;

[MemoryDiagnoser]
public class EmptyBenchmark
{
    private int max = 100000;
    private Counter c = new Counter();

    [Benchmark(Baseline = true)]
    public void CooldownFire()
    {
        var acool = new AsyncCooldown(TimeSpan.Zero, c.Increment);
        for (int i = 0; i < max; i++)
        {
#pragma warning disable CS4014
            acool.Fire();
#pragma warning restore CS4014
        }
    }

    [Benchmark]
    public void DelayQueue()
    {
        var acool = new AsyncDelayedQueue(TimeSpan.Zero, c.Increment);
        for (int i = 0; i < max; i++)
        {
#pragma warning disable CS4014
            acool.Fire();
#pragma warning restore CS4014
        }
    }

    [Benchmark]
    public void DelayDrop()
    {
        var acool = new AsyncDelayedDropped(TimeSpan.Zero, c.Increment);
        for (int i = 0; i < max; i++)
        {
#pragma warning disable CS4014
            acool.Fire();
#pragma warning restore CS4014
        }
    }

    [Benchmark]
    public void DelayLoop()
    {
        var acool = new AsyncLoop(TimeSpan.Zero, c.Increment);
#pragma warning disable CS4014
        acool.Start();
#pragma warning restore CS4014
    }

    [Benchmark]
    public void FloatList()
    {
        List<float> ls = new();
        for (int i = 0; i < max; i++)
        {
            ls.Add(1f);
        }
    }

    [Benchmark]
    public void IntegerListAddRemove()
    {
        List<int> ls = new();
        for (int i = 0; i < max; i++)
        {
            ls.Add(1);
        }

        for (int i = 0; i < 1000; i++)
        {
            ls.Remove(1);
        }
    }
}