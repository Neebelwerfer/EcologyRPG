using System.Collections.Generic;

namespace EcologyRPG.Utility.Collections
{
    public class ListPool<T>
    {
        private readonly Stack<List<T>> stack = new Stack<List<T>>();

        public List<T> Get()
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            return new List<T>();
        }

        public void Add(List<T> list)
        {
            list.Clear();
            stack.Push(list);
        }
    }
}