using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZZ.Common.Logging;
using ZZ.Common.ThirdParty.Log4Net;

namespace ZZ.Common.Socketing
{
    public class ServerSocket
    {
        public EndPoint LocalEndPoint { get; private set; }
        public int Backlog { get; private set; }
        private readonly SocketSetting _socketSetting;
        private readonly Socket _socket;
        private readonly SocketAsyncEventArgs _acceptSocketAsyncEventArgs;
        private readonly ConcurrentDictionary<Guid,TcpConnection> _clientSocketDict;
        private readonly IZLog Log = Log4Netmanager.GetLog();
        public ServerSocket(EndPoint localEP, SocketSetting socketSetting)
        {
            LocalEndPoint = localEP;
            Backlog = socketSetting.Backlog;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      
            _socketSetting = socketSetting;
            
            _acceptSocketAsyncEventArgs = new SocketAsyncEventArgs();
            _acceptSocketAsyncEventArgs.Completed += AcceptSocketAsyncEventArgs_Completed;

            _clientSocketDict = new ConcurrentDictionary<Guid, TcpConnection>();
        }
        public ServerSocket Start()
        {
            Log.InfoFormat("ServerSocket:Start LocalEndPoint:{0}", LocalEndPoint.ToString());
            _socket.Bind(LocalEndPoint);
            _socket.Listen(Backlog);
            _socket.ReceiveBufferSize = _socketSetting.ReceiveBufferSize;
            _socket.ReceiveTimeout = _socketSetting.ReceiveTimeout;
            _socket.SendBufferSize = _socketSetting.SendBufferSize;
            _socket.SendTimeout = _socketSetting.SendTimeout;

            AcceptAsync(_acceptSocketAsyncEventArgs);
            return this;
        }
        private void AcceptAsync(SocketAsyncEventArgs socketAsyncEventArgs)
        {            
            var isAsync=_socket.AcceptAsync(socketAsyncEventArgs);
            if (!isAsync)
            {
                ProcessAccept(socketAsyncEventArgs);
            }
        }
        private void ProcessAccept(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError == SocketError.Success)
            {
                var acceptSocket = socketAsyncEventArgs.AcceptSocket;
                OnProcessAccepted(acceptSocket);
            }
            else
            {
                Log.WarnFormat("ServerSocket:ProcessAccept AcceptSocket.RemoteEndPoint:{0}", socketAsyncEventArgs.AcceptSocket.RemoteEndPoint.ToString());
            }
            socketAsyncEventArgs.AcceptSocket = null;
            AcceptAsync(socketAsyncEventArgs);
        }
        private void AcceptSocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        private void OnProcessAccepted(Socket socket)
        {
            Task.Run(() => {
                TcpConnection tcpConnection = new TcpConnection(socket, _socketSetting);
                _clientSocketDict.TryAdd(Guid.NewGuid(), tcpConnection);
                Log.InfoFormat("ServerSocket:OnProcessAccepted AcceptSocket.RemoteEndPoint:{0}-线程ID-{1}", socket.RemoteEndPoint.ToString(),Thread.CurrentThread.ManagedThreadId);
            });            
        }
    }
}
