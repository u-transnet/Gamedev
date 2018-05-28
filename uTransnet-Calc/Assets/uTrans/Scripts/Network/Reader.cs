using System;
using System.Net.Sockets;
using UnityEngine;

namespace uTrans.Network
{
    public class Reader
    {
        private int bytesNeeded;
        private int bytesRead;
        private NetworkStream networkStream;
        private byte[] byteBuff;
        private Action<byte[]> callback;


        public Reader(NetworkStream networkStream, int bytesNeeded, Action<byte[]> callback)
        {
            this.callback = callback;
            this.networkStream = networkStream;
            this.bytesNeeded = bytesNeeded;
            byteBuff = new byte[bytesNeeded];
            bytesRead = 0;
        }

        public void BeginReading()
        {
            Debug.Log("Start reading");
            networkStream.BeginRead(
                    byteBuff, bytesRead, bytesNeeded - bytesRead,
                    new AsyncCallback(EndReading),
                    networkStream
            );
        }

        public void EndReading(IAsyncResult ar)
        {
            try
            {
                var numberOfBytesRead = networkStream.EndRead(ar);

                if (numberOfBytesRead == 0)
                {
                    Disconnect();
                    return;
                }

                bytesRead += numberOfBytesRead;

                if (bytesRead == bytesNeeded)
                {
                    callback(byteBuff);
                }
                else
                {
                    BeginReading();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Disconnect();
            }
        }

        private void Disconnect()
        {
            Debug.Log("Disconnecting");
        }
    }
}