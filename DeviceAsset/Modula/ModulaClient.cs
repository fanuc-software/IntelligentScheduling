using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Modbus.Net;
using System.IO;

namespace DeviceAsset
{
    public class ModulaClient
    {
        public string IP { get; set; }

        public ushort Port { get; set; }

        private const int MaxCount = 3;
        private TcpClient tcpClient;

        public ModulaClient(string ip, ushort port)
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
                    catch (SocketException ex)
                    {

                    }
                    catch (Exception)
                    { }


                }
            }
            return null;
        }

        private void DisConnect()
        {

        }

        public Tuple<bool, string> Send(string data)
        {
            var tcp = ReConnect();
            if (tcp == null)
            {
                return new Tuple<bool, string>(false, "");
            }

            try
            {
                using (var nsStream = tcp.GetStream())
                {
                    var message = System.Text.Encoding.ASCII.GetBytes(data);
                    nsStream.Write(message, 0, message.Length);
                    // Buffer to store the response bytes.
                    var readData = new Byte[256];
                    
                    Int32 bytes = nsStream.Read(readData, 0, readData.Length);
                    var  responseData = System.Text.Encoding.ASCII.GetString(readData, 0, bytes);
                    return new Tuple<bool, string>(true, responseData);

                }
               
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "");
            }
            
        }


    }
}
