using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace TableSync.Demo
{
    public class NetworkingClient
    {
        public Guid Id;
        public TcpClient Socket;
        public NetworkingServer Server;
        public Thread ClientReceiveThread;

        public NetworkingClient(TcpClient socket, NetworkingServer server)
        {
            Id = Guid.NewGuid();
            Socket = socket; 
            Server = server;
            server.AddClient(this);
        }

        public void Start()
        {
            try
            {
                ClientReceiveThread = new Thread(ListenForData)
                {
                    IsBackground = true
                };
                ClientReceiveThread.Start();
            }
            catch (Exception e)
            {
                Debug.Log($"Client id {Id.ToString()} has thrown the exception:\n" +
                                  $"{e}");
            }
        }

        private void ListenForData()
        {
            try
            {
                var bytes = new byte[1024];
                while (Socket.Connected)
                {
                    using (var stream = Socket.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var data = new byte[length];
                            Array.Copy(bytes, 0, data, 0, length);
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}