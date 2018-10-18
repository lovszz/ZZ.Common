using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZZ.Common.Socketing.BufferManagement
{
    public class BufferPool
    {
        private ConcurrentStack<byte[]> _store;
        private readonly int _maxSize;
        private readonly int _minSize;
        private readonly int _initialSisz;
        private readonly int _bufferSize;
        private int _index = 0;
        public BufferPool(int bufferSize, int initialSisz, int maxSize,int minSize)
        {
            _maxSize = maxSize;
            _bufferSize = bufferSize;
            _initialSisz = initialSisz;
            _minSize = minSize;
            _store = new ConcurrentStack<byte[]>();
            for (int i = 0; i < initialSisz; i++)
            {
                var item = new byte[bufferSize];
                _store.Push(item);
            }
        }
        public byte[] Get()
        {
            if (_store.TryPop(out byte[] result)) return result;
            if (Interlocked.Increment(ref _index) + _initialSisz < _maxSize)
            {
                result = new byte[_bufferSize];
                return result;
            }
            else
            {
                //SpinWait这个对象暂时不清楚具体作用用来替换Thread.Sheep
                var spin = new SpinWait();
                while (true)
                {
                    if (_store.TryPop(out result)) return result;
                    spin.SpinOnce();
                }
            }
        }
        public void Free(byte[] result)
        {
            if (_store.Count > _initialSisz)
            {
                Interlocked.Decrement(ref _index);
                return;
            }                
            Array.Clear(result, 0, result.Length);
            _store.Push(result);
            
        }
    }
}
