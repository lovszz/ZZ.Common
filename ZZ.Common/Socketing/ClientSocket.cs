using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZZ.Common.Logging;
using ZZ.Common.ThirdParty.Log4Net;

namespace ZZ.Common.Socketing
{
    public class ClientSocket
    {
        public EndPoint LocalEP { get; private set; }
        public EndPoint RemoteEP { get; private set; }

        private TcpConnection _tcpConnection;
        private readonly Socket _socket;
        private readonly SocketSetting _socketSetting;
        private readonly SocketAsyncEventArgs _connectSocketAsyncEventArgs;
        /// <summary>
        /// 控制线程 多用于多线程协同和进程操作 https://blog.csdn.net/qq_33303378/article/details/75223615
        /// </summary>
        private readonly ManualResetEvent _manualResetEvent;

        private readonly IZLog Log = Log4Netmanager.GetLog();
        public ClientSocket(EndPoint localEP, EndPoint remoteEP, SocketSetting socketSetting)
        {
            LocalEP = localEP;
            RemoteEP = remoteEP;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);           

            _socketSetting = socketSetting;            

            _connectSocketAsyncEventArgs = new SocketAsyncEventArgs();
            _connectSocketAsyncEventArgs.Completed += ConnectSocketAsyncEventArgs_Completed;

            _manualResetEvent = new ManualResetEvent(false);
        }        
        public ClientSocket Start()
        {
            _socket.Bind(LocalEP);
            _socket.ReceiveBufferSize = _socketSetting.ReceiveBufferSize;
            _socket.ReceiveTimeout = _socketSetting.ReceiveTimeout;
            _socket.SendBufferSize = _socketSetting.SendBufferSize;
            _socket.SendTimeout = _socketSetting.SendTimeout;
            Connect();
            return this;
        }
        public void Send(byte[] message)
        {
            if (_tcpConnection != null)
                _tcpConnection.Send(message);
            else Log.Error("ClientSocket:Send ClientSocket未启动，请运行ClientSocket.Start()");
        }
        private void Connect()
        {
             _connectSocketAsyncEventArgs.RemoteEndPoint = RemoteEP;
            bool isAsync = _socket.ConnectAsync(_connectSocketAsyncEventArgs);
            if (!isAsync)
            {
                ProcessConnect(_connectSocketAsyncEventArgs);
            }
            _manualResetEvent.WaitOne();           
        }
        private void ConnectSocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessConnect(e);
        }
        private void ProcessConnect(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError == SocketError.Success)
            {
                _tcpConnection = new TcpConnection(socketAsyncEventArgs.ConnectSocket, _socketSetting);
                _manualResetEvent.Set();
                Log.InfoFormat("ClientSocket:ProcessConnect--已连接服务器{0}", socketAsyncEventArgs.ConnectSocket.RemoteEndPoint.ToString());
            }
            else
            {
                Log.Error("ClientSocket:ProcessConnect--连接服务错误");
            }
        }
        private void SendHeartBeat()
        {

        }
        
    }
}
