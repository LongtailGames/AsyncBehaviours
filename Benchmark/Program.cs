// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using com.longtailgames.asyncbehaviours;
using AsyncBehaviourTests;

BenchmarkRunner.Run<EmptyBenchmark>();

[MemoryDiagnoser]
public class EmptyBenchmark
{
    private int max = 100000;
    private Counter c = new Counter();

    [Benchmark(Baseline = true)]
    public void cooldownFire()
    {
        var acool = new AsyncCooldown(TimeSpan.Zero, c.Increment);
        for (int i = 0; i < max; i++)
        {
            acool.Fire();
        }
    }

       [Benchmark]
    public void DelayQueue()
    {
        var acool = new AsyncDelayedQueue(TimeSpan.Zero, c.Increment);
        for (int i = 0; i < max; i++)
        {
            acool.Fire();
        }
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