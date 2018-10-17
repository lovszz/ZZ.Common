using System;
using System.Net;
using ZZ.Common.Socketing;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
            var server = new ServerSocket(endPoint, new SocketSetting());
            //需要添加委托事件
            server.Start();
            //EndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8002);
            //var server2 = new ServerSocket(endPoint2, new SocketSetting());
            //server2.Start();
            Console.ReadLine();
        }
        public static long byteArray2Long(byte[] byt, int length)
        {
            long res = 0;
            for (int i = 0; i < length; i++)
            {
                int a = byt[i];
                int b = (int)Math.Pow(16, (length - i - 1) * 2);
                res += a * b;
            }
            return res;
        }
    }
}
