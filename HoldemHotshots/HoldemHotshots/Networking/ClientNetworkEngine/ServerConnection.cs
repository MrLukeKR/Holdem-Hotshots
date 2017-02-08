using System;
using System.Text;
using System.Net.Sockets;

namespace HoldemHotshots
{
    class ServerConnection
    {

        private Socket connection;

        public ServerConnection(Socket connection)
        {
            this.connection = connection;
        }

        public void sendCommand(String command)
        {
            byte[] messageBuffer = Encoding.ASCII.GetBytes(command);
            connection.Send(messageBuffer);
        }

        public String getResponse()
        {
            Byte[] Buffer;
            Buffer = new Byte[255];
            int messageSize = connection.Receive(Buffer, 0, Buffer.Length, 0);
            Array.Resize(ref Buffer, messageSize);
            String response = Encoding.Default.GetString(Buffer);
            return response;
        }



    }
}
