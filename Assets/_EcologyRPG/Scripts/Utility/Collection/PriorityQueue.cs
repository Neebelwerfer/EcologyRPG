using System.Collections.Generic;

namespace Utility.Collections
{
    public enum Priority
    {
        VeryHigh,
        High,
        Normal,
        Low,
        VeryLow,
    }

    public class PriorityQueue<T> where T : class
    {
        const uint priorityFlipRate = 5;
        readonly Dictionary<uint, Queue<T>> queue = new();

        public uint Count { get => count; }

        uint count = 0;
        uint frameCountSincePriorityFlip = 0;
        public PriorityQueue()
        {
            for (uint i = 0; i < 5; i++)
            {
                queue.Add(i, new Queue<T>());
            }

        }

        public void Enqueue(T item, Priority priority)
        {
            queue[(uint)priority].Enqueue(item);
            count++;
        }

        public T Dequeue()
        {
            if (count == 0)
            {
                frameCountSincePriorityFlip = 0;
                return null;
            }

            if (frameCountSincePriorityFlip >= priorityFlipRate)
            {
                frameCountSincePriorityFlip = 0;
                for (uint i = 4; i >= 0; i--)
                {
                    if (queue[i].Count > 0)
                    {
                        count--;
                        return queue[i].Dequeue();
                    }
                }
            }

            frameCountSincePriorityFlip++;
            for (uint i = 0; i < 5; i++)
            {
                if (queue[i].Count > 0)
                {
                    count--;
                    return queue[i].Dequeue();
                }
            }
            return null;
        }
    }
}