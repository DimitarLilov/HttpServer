namespace HttpServer
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Models;

    public class Server
    {
        public Server(int port, IEnumerable<Route> routes)
        {
            this.Port = port;
            this.Processor = new HttpProcessor(routes);
            this.IsActive = true;
        }

        public TcpListener Listener { get; private set; }

        public int Port { get; set; }

        public HttpProcessor Processor { get; private set; }

        public bool IsActive { get; private set; }

        public void Listen()
        {
            this.Listener = new TcpListener(IPAddress.Any, this.Port);
            this.Listener.Start();
            while (this.IsActive)
            {
                TcpClient client = this.Listener.AcceptTcpClient();
                Thread thread = new Thread(() =>
                {
                    this.Processor.HendleClient(client);
                });
                thread.Start();
                Thread.Sleep(1);
            }
        }
    }
}
