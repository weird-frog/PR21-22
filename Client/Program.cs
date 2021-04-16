using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\nСоединение # " + i.ToString() + "\n");
                Connect("127.0.0.1", "Hello World! #" + i.ToString());
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadKey();
        }
        static void Connect(String server, String message)
        {
            try
            {
                Int32 prot = 9595;
                TcpClient client = new TcpClient(server, prot);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Отправлено: {message}");

                data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine($"Получено: {responseData}");
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException: {ex}");
            }
            catch(SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex}");
            }
        }
    }
}
