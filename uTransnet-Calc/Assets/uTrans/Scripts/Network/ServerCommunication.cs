namespace uTrans.Network
{
    using System;
    using UnityEngine;
    using uTrans.Proto;

    public class ServerCommunication
    {
        public static void SendEnvelope(Envelope request, Action<Envelope> callback)
        {
            TCPClient tcpClient = new TCPClient();
            tcpClient.ConnectToTcpServer(() =>
            {
                tcpClient.SendMessage(request, response =>
                {
                    callback(response);
                    tcpClient.Disconnect();
                });
            });
        }

        public static void SendName(string name)
        {
            Envelope request = new Envelope();
            request.Id = 1;
            request.Name = name;
            SendEnvelope(request, response =>
            {
                Debug.Log("Server answered: " + response.Name);
            });
        }

    }
}