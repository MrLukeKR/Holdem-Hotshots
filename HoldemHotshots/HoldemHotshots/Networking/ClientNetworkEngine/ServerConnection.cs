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
            byte[] prefix = new byte[4];

            //send prefix
            prefix = BitConverter.GetBytes(messageBuffer.Length);
            connection.Send(prefix);

            //send actual message
            connection.Send(messageBuffer);
        }

        public String getResponse()
        {
            byte[] prefix = new byte[4];

            //read prefix
            connection.Receive(prefix,0,4,0);
            int messagelength = BitConverter.ToInt32(prefix, 0);

            //read actual message
            Byte[] Buffer = new byte[messagelength];
            connection.Receive(Buffer, 0, messagelength, 0);
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

        public void sendFold()
        {
            sendAction("FOLD");
        }

        public void sendRaise(uint amount)
        {
            sendAction("RAISE:" + amount);
        }

        public void sendCheck()
        {
            sendAction("CHECK");
        }

        public void sendAllIn()
        {
            sendAction("ALL_IN");
        }

        public void sendCall()
        {
            sendAction("CALL");
        }
    }
}
