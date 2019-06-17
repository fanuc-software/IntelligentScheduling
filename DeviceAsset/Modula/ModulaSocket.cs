using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace DeviceAsset.Modula
{
    public class ModulaSocket
    {
        public string IP { get; set; }

        public ushort Port { get; set; }

        private const int MaxCount = 3;
        private TcpClient tcpClient;
        public ModulaSocket(string ip, ushort port)
        {
            IP = ip;
            Port = port;

        }


        private TcpClient ReConnect()
        {
            if (tcpClient?.Connected ?? false)
            {
                return tcpClient;
            }
            for (int i = 0; i < MaxCount; i++)
            {
                if (tcpClient == null || !tcpClient.Connected)
                {
                    try
                    {
                        tcpClient = new TcpClient(IP, Port);
                        return tcpClient;

                    }
                    catch (SocketException)
                    {

                    }
                    catch (Exception)
                    { }


                }
            }
            return null;
        }

        public bool Send(string data)
        {
            var tcp = ReConnect();
            if (tcp == null)
            {
                return false;
            }
            using (var stream = new System.IO.StreamWriter(tcp.GetStream()))
            {
                try
                {
                    stream.Write(data);

                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return true;
        }

        public Tuple<bool, string> Receive()
        {
            var tcp = ReConnect();
            if (tcp == null)
            {
                return new Tuple<bool, string>(false,"");
            }
            using (var stream = new System.IO.StreamReader(tcp.GetStream()))
            {
                try
                {
                   return new Tuple<bool, string>(true,  stream.ReadLine());

                }
                catch (ObjectDisposedException)
                {
                    return new Tuple<bool, string>(false, "");
                }
                catch (Exception)
                {

                    return new Tuple<bool, string>(false, "");
                }
            }
        }

    }
}
