using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TableSync.Demo
{
    public class NetworkingServer
    {
        public TcpListener TcpListener;
        public readonly List<NetworkingClient> Clients = new List<NetworkingClient>();

        public void AddClient(NetworkingClient networkingClient)
        {
            Clients.Add(networkingClient);
        }

        public IEnumerator WaitForClientToConnect()
        {
            TcpListener = new TcpListener(IPAddress.Any, 8888);
            TcpListener.Start();
            var task = TcpListener.AcceptTcpClientAsync();
            yield return task;
            var client = new NetworkingClient(task.Result, this);
            client.Start();
        }
    }
}