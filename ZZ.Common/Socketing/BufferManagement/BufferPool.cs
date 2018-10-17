using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZZ.Common.Socketing.BufferManagement
{
    /// <summary>
    /// 缓冲池满的处理方式
    /// </summary>
    public enum BufferFullDeal
    {
        /// <summary>
        /// 继续添加
        /// </summary>
        CONTINUE = 1,
        /// <summary>
        /// 返回失败
        /// </summary>
        RETURN = 2,
        /// <summary>
        /// 抛出异常
        /// </summary>
        THROW = 3
    }

    public class BufferPool
    {
        private ConcurrentStack<byte[]> _store;
        private Int64 maxCount;
        private BufferFullDeal bufferFullDeal;

        public BufferPool(Int64 initialCount, BufferFullDeal initialFullDeal = BufferFullDeal.CONTINUE)
        {
            _store = new ConcurrentStack<byte[]>();
            maxCount = initialCount;
            bufferFullDeal = initialFullDeal;
        }

        public bool TryPop(out byte[] result)
        {
            return _store.TryPop(out result);
        }

        public Boolean Push(byte[] result)
        {
            //栈池已满
            if (_store.Count > maxCount)
            {
                switch (bufferFullDeal)
                {
                    case BufferFullDeal.CONTINUE:
                        break;
                    case BufferFullDeal.RETURN:
                        return false;
                    case BufferFullDeal.THROW:
                        throw new OverflowException("Buffer pool is full");
                    default:
                        break;
                }
            }
            _store.Push(result);
            return true;
        }
    }
}
