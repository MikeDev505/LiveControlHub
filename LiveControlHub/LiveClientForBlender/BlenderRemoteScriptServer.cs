using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LiveClientForBlender
{
    public class BlenderRemoteScriptServer
    {
        TcpListener server;
        NetworkStream channel;
        byte[] buffer = new byte[4];


        public void Start()
        {
            server = new TcpListener(IPAddress.Any, 56788);
            server.Start();
            Accept();
            var response = RunScript("scriptResult = 'test ok'");
        }

        public void Accept()
        {
            var client = server.AcceptTcpClient();
            channel = client.GetStream();
            var startHeader = ReciveMessageString();
        }

        public string RunScript(string script)
        {
            SendMessageString(script);
            var response = ReciveMessageString();
            return response;
        }

        public void SendMessageString(string msg)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(msg);
            SendMessageBytes(bytes);
        }

        public void SendMessageBytes(byte[] message)
        {
            var messageSize = message.Length;
            var bufor = BitConverter.GetBytes(messageSize);
            channel.Write(bufor,0 , 4); //wysyłam rozmiar
            int totalSended = 0;
            //while(true)
            {
                var sizeToSend = messageSize - totalSended;
                channel.Write(message, totalSended, sizeToSend);
            }
        }

        public string ReciveMessageString()
        {
            var bytes = ReciveMessageBytes();
            var msg = System.Text.Encoding.UTF8.GetString(bytes);
            return msg;
        }

        public byte[] ReciveMessageBytes()
        {
            channel.Read(buffer, 0, 4);
            var messageSize = BitConverter.ToInt32(buffer, 0);
            var messageBuffer = new byte[messageSize];
            int totalRead = 0;

            while (true)
            {
                var leftToRead = messageSize - totalRead;
                var packetSize = channel.Read(messageBuffer, totalRead, leftToRead);
                if (packetSize < 0)
                {
                    //błąd
                    return null;
                }

                totalRead += packetSize;
                if (totalRead == messageSize)
                {
                    return messageBuffer;
                }
            }
        }
    }
}
