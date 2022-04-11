namespace AsyncBehaviourTests
{
    public class Counter
    {
        public int Count { get; private set; }

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