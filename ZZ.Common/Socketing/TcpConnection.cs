using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZZ.Common.Socketing
{
    public class TcpConnection
    {
        private readonly Socket _socket;
        private readonly SocketAsyncEventArgs _receiveSocketAsyncEvent;
        private readonly SocketAsyncEventArgs _sendSocketAsyncEvent;
        private readonly ConcurrentQueue<byte[]> _sendQueue;
        
        private int _sending = 0;
        private int _receiving = 0;
        public TcpConnection(Socket socket, SocketSetting socketSeting)
        {
            _socket = socket;            

            _sendSocketAsyncEvent = new SocketAsyncEventArgs();
            _sendSocketAsyncEvent.AcceptSocket = socket;
            _sendSocketAsyncEvent.Completed += SendSocketAsyncEvent_Completed;

            _receiveSocketAsyncEvent = new SocketAsyncEventArgs();
            _receiveSocketAsyncEvent.AcceptSocket = socket;
            _receiveSocketAsyncEvent.Completed += ReceiveSocketAsyncEvent_Completed;

            _sendQueue = new ConcurrentQueue<byte[]>();
        }      
        public void Send(byte[] message)
        {
            _sendQueue.Enqueue(message);
            TrySend();
        }
        private void TrySend()
        {
            if (!EnterSending()) return;           
            byte[] message;
            if (_sendQueue.TryDequeue(out message))
            {
                _sendSocketAsyncEvent.SetBuffer(message,0, message.Length);
                var isAsync = _sendSocketAsyncEvent.AcceptSocket.SendAsync(_sendSocketAsyncEvent);
                if (!isAsync)
                {
                    ProcessSend(_sendSocketAsyncEvent);
                }
            }
        }
        private void ProcessSend(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            ExitSending();
            if (socketAsyncEventArgs.SocketError == SocketError.Success)
            {                
                TrySend();
            }
            else
            {
                //抛异常
            }            
        }
        private bool EnterSending()
        {
            if (Interlocked.CompareExchange(ref _sending, 1, 1) == 1) return false;
            Interlocked.Exchange(ref _sending, 1);
            return true;
        }
        private void ExitSending()
        {
            Interlocked.Exchange(ref _sending, 0);
        }
        private void SendSocketAsyncEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessSend(e);
        }
        private void TryReceive()
        {
          
        }
        private void ProcessReceive(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            //Task.Factory.
        }
        private bool EnterReceiving()
        {
            if (Interlocked.CompareExchange(ref _receiving, 1, 1) == 1) return false;
            Interlocked.Exchange(ref _receiving, 1);
            return true;
        }
        private void ExitReceiving()
        {
            Interlocked.Exchange(ref _receiving, 0);
        }
        private void ReceiveSocketAsyncEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            throw new NotImplementedException();
        } 
    }
}
