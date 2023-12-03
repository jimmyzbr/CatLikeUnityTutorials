using System;
using System.Collections.Generic;

namespace EasyKit
{
    public class CycleBuffer<T> where T : IPooledObject, new()
    {
        private Queue<T> mCache;

        public int count
        {
            get { return mCache.Count; }
        }

        public CycleBuffer(int cap = 20)
        {
            mCache = new Queue<T>(cap);
            for (int i = 0; i < cap; i++)
            {
                T data = Activator.CreateInstance<T>();
                data.Init(null);
                AddElement(data);
            }
        }

        public T AcquireElement()
        {
            T result = default(T);
            bool flag = mCache.Count > 0;
            if (flag)
            {
                result = mCache.Dequeue();
            }
            else
            {
                result = Activator.CreateInstance<T>();
                result.Init(null);
            }

            result.Spawning();
            return result;
        }

        public void recycle(T data)
        {
            AddElement(data);
        }

        public void Clear()
        {
            mCache.Clear();
        }

        private void AddElement(T data)
        {
            mCache.Enqueue(data);
        }
    }
}