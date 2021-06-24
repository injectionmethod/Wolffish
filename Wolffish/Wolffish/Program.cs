using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Wolffish
{
    class Program
    {
        static void Main(string[] args)
        {
            Handler();
        }

        static void Handler()
        {
            RandomGenerator generator = new RandomGenerator();         
            TcpListener server = null;
            try
            {
                Int32 port = 0;
                IPAddress localAddr = IPAddress.Parse(IPAddress.Any.ToString());
                server = new TcpListener(localAddr, port);
                server.Start();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.WriteLine("Sinkhole is listening for a connection on  " + localAddr + ":" + port);
                    TcpClient client = server.AcceptTcpClient();

                    char[] separator = new char[] { ':' };
                    string[] splitter = client.Client.RemoteEndPoint.ToString().Split(separator);


                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + splitter[0]);
                    StreamWriter SW = new StreamWriter(Environment.CurrentDirectory + "\\" + splitter[0] + "\\ # "+ generator.RandomNumber(10000000, 99999999).ToString() + "-Pass");
                    Console.WriteLine("Client " + client.Client.RemoteEndPoint.ToString() +" Connected!");
                    data = null;
                    int i;
                    NetworkStream stream = client.GetStream();                                    
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {                          
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);                       
                        Console.WriteLine(data);
                        SW.WriteLine(data);
                    }
                    if (client.Connected == true)
                    {
                        SW.Close();
                        client.Close();
                    }
                    else
                    {
                        SW.Close();
                    }
                }
            }
            catch (SocketException e)
            {
                server.Stop();
                Console.WriteLine("SocketException: {0}", e);
                Handler();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
    public class RandomGenerator
    {
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
