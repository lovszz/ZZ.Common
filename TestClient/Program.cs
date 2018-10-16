using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZZ.Common.Socketing;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);           
            EndPoint localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);           
            var client = new ClientSocket(localEP, endPoint, new SocketSetting());
            client.Start();
            while(true)
            {
                var message = Console.ReadLine();
                if (string.IsNullOrEmpty(message)) message = "0";
                client.Send(getBytes(int.Parse(message), true));
                Console.WriteLine("已发送");
            }            
        }
        public static byte[] getBytes(int s, bool asc)
        {
            byte[] buf = new byte[4];
            if (asc)
                for (int i = buf.Length - 1; i >= 0; i--)
                {
                    buf[i] = (byte)(s & 0x000000ff);
                    s >>= 8;
                }
            else
                for (int i = 0; i < buf.Length; i++)
                {
                    buf[i] = (byte)(s & 0x000000ff);
                    s >>= 8;
                }
            return buf;
        }
    }
}
