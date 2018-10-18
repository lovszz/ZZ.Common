using System;
using System.Collections.Generic;
using System.Text;

namespace ZZ.Common.Socketing
{
    public class SocketSetting
    {
        public int Backlog => 100;
        //
        // 摘要:
        //     Gets or sets a value that specifies the size of the receive buffer of the System.Net.Sockets.Socket.
        //
        // 返回结果:
        //     An System.Int32 that contains the size, in bytes, of the receive buffer. The
        //     default is 8192.
        //
        // 异常:
        //   T:System.Net.Sockets.SocketException:
        //     An error occurred when attempting to access the socket.
        //
        //   T:System.ObjectDisposedException:
        //     The System.Net.Sockets.Socket has been closed.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value specified for a set operation is less than 0.
        public int ReceiveBufferSize =>4;
        //
        // 摘要:
        //     Gets or sets a value that specifies the amount of time after which a synchronous
        //     Overload:System.Net.Sockets.Socket.Receive call will time out.
        //
        // 返回结果:
        //     The time-out value, in milliseconds. The default value is 0, which indicates
        //     an infinite time-out period. Specifying -1 also indicates an infinite time-out
        //     period.
        //
        // 异常:
        //   T:System.Net.Sockets.SocketException:
        //     An error occurred when attempting to access the socket.
        //
        //   T:System.ObjectDisposedException:
        //     The System.Net.Sockets.Socket has been closed.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value specified for a set operation is less than -1.
        public int ReceiveTimeout => 10;
        //
        // 摘要:
        //     Gets or sets a value that specifies the size of the send buffer of the System.Net.Sockets.Socket.
        //
        // 返回结果:
        //     An System.Int32 that contains the size, in bytes, of the send buffer. The default
        //     is 8192.
        //
        // 异常:
        //   T:System.Net.Sockets.SocketException:
        //     An error occurred when attempting to access the socket.
        //
        //   T:System.ObjectDisposedException:
        //     The System.Net.Sockets.Socket has been closed.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value specified for a set operation is less than 0.
        public int SendBufferSize => 4;
        //
        // 摘要:
        //     Gets or sets a value that specifies the amount of time after which a synchronous
        //     Overload:System.Net.Sockets.Socket.Send call will time out.
        //
        // 返回结果:
        //     The time-out value, in milliseconds. If you set the property with a value between
        //     1 and 499, the value will be changed to 500. The default value is 0, which indicates
        //     an infinite time-out period. Specifying -1 also indicates an infinite time-out
        //     period.
        //
        // 异常:
        //   T:System.Net.Sockets.SocketException:
        //     An error occurred when attempting to access the socket.
        //
        //   T:System.ObjectDisposedException:
        //     The System.Net.Sockets.Socket has been closed.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value specified for a set operation is less than -1.
        public int SendTimeout => 10;
        /// <summary>
        /// 缓存池大小配置
        /// </summary>
        public int ReceiveDataBufferPoolSize => 100;
        /// <summary>
        /// 缓存池最大数量配置
        /// </summary>
        public int ReceiveDataBufferPoolMaxSize => 1000;
        /// <summary>
        /// 缓存池最小数量配置
        /// </summary>
        public int ReceiveDataBufferPoolMinSize => 1000;
    }
}
