using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HoldemHotshots
{
    class ClientConnectionMonitorThread
    {
        private static bool receivedCommandRecently = true;
        private static int  lastCommandCountdown = 5;
        private static int  timeoutCountdown = 5;
        private static Thread timeoutTimer = new Thread(StartTimeout);
        private Socket connectionSocket;

        public ClientConnectionMonitorThread(Socket connectionSocket)
        {
            this.connectionSocket = connectionSocket;
        }

        public void start()
        {
            var monitor = new Thread(monitorConnection);
            Console.WriteLine("Startng connection monitor");
            monitor.Start();
            timeoutTimer.Start();
        }

        private void Ping()
        {
                byte[] messageBuffer = Encoding.ASCII.GetBytes("PING");
                byte[] prefix = new byte[4];

                //send prefix
                prefix = BitConverter.GetBytes(messageBuffer.Length);
            try
            {
                connectionSocket.Send(prefix);

                //send actual message
                connectionSocket.Send(messageBuffer);
            }
            catch { }
        }

        public static void ResetCommandTimer()
        {
            receivedCommandRecently = true;
            lastCommandCountdown = 5;
            timeoutCountdown = 5;
        }

        private static void StartTimeout(){
            while (true)
            {
                while (lastCommandCountdown-- > 0) Thread.Sleep(1000);
                receivedCommandRecently = false;
            }
        }
        
        private void monitorConnection()
        {
            EndPoint reconnectEP = connectionSocket.RemoteEndPoint;
            while (timeoutCountdown > 0)
            {
                if (!receivedCommandRecently) {
                    Ping();
                    timeoutCountdown--;
                }

                Thread.Sleep(1000);

                if(timeoutCountdown < 5 && timeoutCountdown > 0) Console.WriteLine("Timing out in " + timeoutCountdown);
            }

            Console.WriteLine("Timed out!");
            handleDisconnect();
        }

        private bool AttemptReconnect(EndPoint connectionPoint)
        {
            Console.WriteLine("Attemping to reconnect...");
            try
            {
                //TODO: Reconnect code (Listener?)
                //connectionSocket.Disconnect(true);
                //connectionSocket.Connect(connectionPoint);
            }catch
            {
                Console.WriteLine("Failed to reconnect");
            }
            //TODO: Reconnection code

            return connectionSocket.Connected;
        }

        private void handleDisconnect()
        {
            //TODO: Handle disconnect
            Room.CheckConnections();
        }
    }
}
