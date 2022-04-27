using System.Threading.Tasks;

namespace AsyncBehaviourTests
{
    public class Counter
    {
        public int Count { get; private set; }

        public Task IncrementAsync()
        {
            Increment();
            return Task.CompletedTask;
        }
        public void Increment()
        {
            Count++;
        }

        public void Reset()
        {
            Count = 0;
        }
    }
}