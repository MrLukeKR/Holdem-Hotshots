using System;
using System.Text;
using System.Net.Sockets;

namespace HoldemHotshots
{
    class ServerConnection : ServerInterface
    {

        private Socket connection;
        private ServerConnectionMonitorThread monitorThread;

        public ServerConnection(Socket connection)
        {
            this.connection = connection;
            this.monitorThread = new ServerConnectionMonitorThread(connection);
            monitorThread.start();
        }

        public void sendMessage(String command)
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

        public void sendAction(String action)
        {
            /*
             * Use this command after the server has asked for a action
             */
            sendMessage(action);
        }



    }
}
