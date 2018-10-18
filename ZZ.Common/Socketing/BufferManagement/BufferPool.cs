using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZZ.Common.Socketing.BufferManagement
{
    public class BufferPool
    {
        private ConcurrentStack<byte[]> _store;

        public BufferPool(int bufferSize, int initialCount)
        {
            _store = new ConcurrentStack<byte[]>();
            for (int i = 0; i < initialCount; i++)
            {
                var item = new byte[bufferSize];
                _store.Push(item);
            }
        }
        public bool TryGet(out byte[] result)
        {
            return _store.TryPop(out result);
        }
        public void Free(ref byte[] result)
        {
            Array.Clear(result, 0, result.Length);
            _store.Push(result);
        }
    }
}
