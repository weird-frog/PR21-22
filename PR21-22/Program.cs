using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PR21_22
{                                  
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int maxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
                ThreadPool.SetMaxThreads(2, 2);

                Int32 port = 9595;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);

                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine("Конфигурация многопоточного сервера:");
                Console.WriteLine("\tIP-адрес:\t127.0.0.1");
                Console.WriteLine("\tПорт:\t" + port.ToString());
                Console.WriteLine("\tПотоки:\t" + maxThreadsCount.ToString());
                Console.WriteLine("\nСервер запущен\n");

                server.Start();
                while (true)
                {
                    Console.WriteLine("\nОжидание соединения...");
                    ThreadPool.QueueUserWorkItem(ClientProcessing, server.AcceptTcpClient());
                    counter++;
                    Console.WriteLine("\nСоединение #" + counter.ToString() + "!");
                }
            }
            catch (SocketException ex)
            {

                Console.WriteLine($"SocketException: {ex}");
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadKey();
        }

        private static void ClientProcessing(object client_obj)
        {
            Byte[] bytes = new byte[256];
            String data = null;
            TcpClient client = client_obj as TcpClient;

            data = null;
            NetworkStream stream = client.GetStream();
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                data = data.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                stream.Write(msg, 0, msg.Length);
            }
            client.Close();
        }
    }
}
