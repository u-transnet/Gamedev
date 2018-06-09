namespace uTrans.Network
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using Google.Protobuf;
    using UnityEngine;
    using uTrans.Proto;

    public class TCPClient
    {
        private TcpClient socketConnection;
        private Thread clientReceiveThread;
        private bool connected = false;

        private event Action OnConnect;

        private Queue<Action<Envelope>> responseCallbacks = new Queue<Action<Envelope>>(2);

        /// <summary>
        /// Setup socket connection.
        /// </summary>
        public void ConnectToTcpServer(Action onConnectCallback)
        {
            OnConnect = onConnectCallback;
            try
            {
                clientReceiveThread = new Thread(new ThreadStart(Connect));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Name = "TCPClient";
                clientReceiveThread.Start();
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
            }
        }

        private void Connect()
        {
            socketConnection = new TcpClient("192.168.1.172", 8463);
            connected = true;
            OnConnect();
        }

        /// <summary>
        /// Listens for incomming data.
        /// </summary>
        private void ListenForData()
        {
            try
            {
                // Get a stream object for reading
                NetworkStream stream = socketConnection.GetStream();
                stream.ReadTimeout = 10000;

                Debug.Log("Receiving message");
                // Read length
                int length = NetworkUtils.DirectReadVarintInt32(stream);
                Debug.Log("Message length: " + length);
                Reader reader = new Reader(
                        stream,
                        length,
                        bytes =>
                        {
                            Envelope envelope = Envelope.Parser.ParseFrom(bytes);
                            HandleMessage(envelope);
                        }
                );
                reader.Read();
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


        private void HandleMessage(Envelope envelope)
        {
            Debug.Log("Received message: " + envelope.ToString());
            Action<Envelope> callback = responseCallbacks.Dequeue();
            callback(envelope);
        }

        /// <summary>
        /// Send bytes to server using socket connection.
        /// </summary>
        private void SendMessage(byte[] data)
        {
            if (socketConnection == null)
            {
                Debug.Log("No connection");
                return;
            }
            try
            {
                // Get a stream object for writing.
                NetworkStream stream = socketConnection.GetStream();
                if (stream.CanWrite)
                {
                    // Write length of message.
                    CodedOutputStream codedStream = new CodedOutputStream(stream, true);
                    //                    int headerLength = CodedOutputStream.ComputeInt32Size(data.Length);
                    codedStream.WriteInt32(data.Length);
                    codedStream.Flush();
                    // Write byte array to socketConnection stream.
                    stream.Write(data, 0, data.Length);
                    Debug.Log("Message sended");
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

        /// <summary>
        /// Convert message into bytes and send it to server using socket connection.
        /// </summary>
        public void SendMessage(Request request, Action<Envelope> onResponse = null)
        {
            Debug.Log("Sending message");
            MemoryStream ms = new MemoryStream();
            CodedOutputStream codedStream = new CodedOutputStream(ms);
            request.WriteTo(codedStream);
            codedStream.Flush();
            SendMessage(ms.ToArray());

            if (onResponse != null)
            {
                responseCallbacks.Enqueue(onResponse);
                ListenForData();
            }
            else
            {
                responseCallbacks.Enqueue(ignore =>
                {
                });
            }
        }

        public bool IsConnected()
        {
            return connected;
        }

        public void Disconnect()
        {
            Debug.Log("Disconnecting");
            socketConnection.Close();
            connected = false;
        }
    }
}