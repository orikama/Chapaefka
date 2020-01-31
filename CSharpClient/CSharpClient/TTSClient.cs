﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Buffers;

// https://codereview.stackexchange.com/questions/177526/c-async-socket-wrapper

namespace CSharpClient.TTSServerSocket
{
    public class TTSClient : IDisposable
    {
        private const int PrefixLength = 4;
        private const int SendBufferSize = 512;
        private const int ReceiveBufferSize = 32768;

        private Socket _socket = null;
        private string _ttsServerNameOrAddress = null;
        private int _ttsServerPort = -1;
        private IPEndPoint _ipEndPoint;

        private const int WavHeaderOffset = 44;
        private const int WavSecondSize = 44100; // 22050Hz * 2 bytes
        private const int MaxSeconds = 150;
        private const int WavBufferSize = WavSecondSize * MaxSeconds;
        private bool _bufferIsFull = false;

        //private readonly ConcurrentQueue<byte[]> _rawWavs = new ConcurrentQueue<byte[]>();
        private readonly ArrayPool<byte> _wavBuffer = ArrayPool<byte>.Create(WavBufferSize, 10);
        private int _wavBufferBytesRented = 0;

        private AsyncCallback _nullCallback = (i) => { };

        private bool _disposed = false;


        //public event Action FreeSpaceInBuffer;

        public bool Connected
        {
            get { return _socket is null ? false : _socket.Connected; }
        }

        public bool BufferIsFull => _bufferIsFull;

        //public ConcurrentQueue<byte[]> RawWavs => _rawWavs;


        public TTSClient()
        { }

        public async Task ConnectAsync(string ttsServerNameOrAddress, int ttsServerPort)
        {
            bool endPointChanged = false;

            IPHostEntry ipHostInfo = null;

            if (_ttsServerNameOrAddress != ttsServerNameOrAddress)
            {
                _ttsServerNameOrAddress = ttsServerNameOrAddress;
                ipHostInfo = await Dns.GetHostEntryAsync(ttsServerNameOrAddress).ConfigureAwait(false);

                endPointChanged = true;
            }
            if (_ttsServerPort != ttsServerPort)
            {
                _ttsServerPort = ttsServerPort;
                endPointChanged = true;
            }

            if (endPointChanged)
            {
                _ipEndPoint = new IPEndPoint(ipHostInfo.AddressList[0], ttsServerPort);
                _socket = new Socket(_ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.ReceiveBufferSize = ReceiveBufferSize;
                _socket.SendBufferSize = SendBufferSize;
                //_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress)
            }

            await Task.Factory.FromAsync(_socket.BeginConnect, _socket.EndConnect, _ipEndPoint, null).ConfigureAwait(false);
        }

        public async Task SendAsync(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            
            IAsyncResult result = _socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, _nullCallback, null);
            
            await Task.Factory.FromAsync(result, _socket.EndSend).ConfigureAwait(false);
        }

        public async Task<Tuple<int, byte[]>> ReceiveAsync()
        {
            byte[] prefix = new byte[PrefixLength];

            int recvLen = await Task.Factory.FromAsync(
                                   (cb, s) => _socket.BeginReceive(prefix, 0, PrefixLength, SocketFlags.None, cb, s),
                                   ias => _socket.EndReceive(ias),
                                   null).ConfigureAwait(false);

            if (recvLen != PrefixLength)
            {
                throw new ApplicationException("Failed to receive prefix");
            }

            int contentLength = BitConverter.ToInt32(prefix, 0);
            byte[] bufferReference = _wavBuffer.Rent(contentLength + WavHeaderOffset);
            //_rawWavs.Enqueue(buffer);

            _wavBufferBytesRented += bufferReference.Length;
            if (_wavBufferBytesRented + WavSecondSize * 20 >= WavBufferSize)
                _bufferIsFull = true;

            int currentLength = 0;
            while(currentLength < contentLength)
            {
                recvLen = await Task.Factory.FromAsync(
                                   (cb, s) => _socket.BeginReceive(bufferReference, currentLength + WavHeaderOffset, contentLength - currentLength, SocketFlags.None, cb, s),
                                   ias => _socket.EndReceive(ias),
                                   null).ConfigureAwait(false);

                currentLength += recvLen;
            }

            if (currentLength != contentLength)
                throw new ApplicationException("Failed to receive content");

            return new Tuple<int, byte[]>(contentLength + WavHeaderOffset, bufferReference);
        }
        // My fellow americans, today i found out that Igor Nazarov is a god damn cocksucker
        public async Task DisconnectAsync()
        {
            _socket.Shutdown(SocketShutdown.Both);
            await Task.Factory.FromAsync(_socket.BeginDisconnect, _socket.EndDisconnect, true, null).ConfigureAwait(false);
        }

        public void ReturnToBuffer(byte[] buffer)
        {
            _wavBufferBytesRented -= buffer.Length;
            _wavBuffer.Return(buffer);

            if (_wavBufferBytesRented + WavSecondSize * 20 <= WavBufferSize)
            {
                _bufferIsFull = false;
                //FreeSpaceInBuffer?.Invoke();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            //_socket?.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _socket?.Dispose();

            _disposed = true;
        }

        ~TTSClient()
        {
            Dispose(false);
        }
    }

}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using System.Threading;

////using NAudio.Wave;
//using System.IO;

//using System.Net;
//using System.Net.Sockets;

//namespace CSharpClient
//{
//    public class StateObject
//    {
//        // Client socket.  
//        public Socket socket = null;
//        // Size of receive buffer.  
//        public byte[] length = new byte[4];

//        public int currentLength = 0;
//        // Receive buffer.  
//        public byte[] buffer = null;
//        // Received data string.  
//        //public StringBuilder sb = new StringBuilder();
//    }

//    class TTSClient
//    {

//        private Socket _socket = null;
//        private string _ttsServerNameOrAddress = null;
//        private int _ttsServerPort = -1;
//        private IPEndPoint _ipEndPoint;

//        private const int OpsToPreAlloc = 2;

//        private const int ReceivePrefixLength = 4;
//        private const int SendPrefixLength = 4;

//        private const int SendBufferSize = 512;
//        private const int ReceiveBufferSize = 32768;

//        // ManualResetEvent instances signal completion.  
//        public static ManualResetEvent connectDone = new ManualResetEvent(false);
//        public static ManualResetEvent sendDone = new ManualResetEvent(false);
//        public static ManualResetEvent receiveDone = new ManualResetEvent(false);

//        // The response from the remote device.  
//        private static string response = string.Empty;

//        public TTSClient()
//        {
//            try
//            {


//                //_socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//                _socket = new Socket(AddressFamily., SocketType.Stream, ProtocolType.Tcp);
//                _socket.SendBufferSize = SendBufferSize;
//                _socket.ReceiveBufferSize = ReceiveBufferSize;
//                //_socket.receive
//                //_socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), _socket);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        ~TTSClient()
//        {
//            _socket.Shutdown(SocketShutdown.Both);
//            _socket.Close();
//        }

//        public bool IsConnected => _socket.Connected;

//        public void Connect(string ttsServerNameOrAddress, int ttsServerPort)
//        {
//            if(ttsServerNameOrAddress != string.Empty)

//            if(_ttsServerNameOrAddress != ttsServerNameOrAddress || _ttsServerPort != ttsServerPort)
//            {
//                    Dns.get
//            }


//            IPHostEntry ipHostInfo = Dns.GetHostEntry(TTSServerAddress);
//            IPAddress ipAddress = ipHostInfo.AddressList[0];
//            _ipEndPoint = new IPEndPoint(ipAddress, port);

//            _socket.BeginConnect(_ipEndPoint, new AsyncCallback(ConnectCallback), _socket);
//        }

//        public void Send(string data)
//        {
//            // Convert the string data to byte data using ASCII encoding.  
//            byte[] byteData = Encoding.UTF8.GetBytes(data);

//            // Begin sending the data to the remote device.  
//            _socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), _socket);
//        }

//        public void Receive()
//        {
//            try
//            {
//                // Create the state object.  
//                StateObject state = new StateObject();
//                state.socket = _socket;

//                // Begin receiving the data from the remote device.
//                _socket.BeginReceive(state.length, 0, state.length.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
//                //_socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        //private void StartClient()
//        //{
//        //    // Connect to a remote device.  
//        //    try
//        //    {

//        //        connectDone.WaitOne();

//        //        // Send test data to the remote device.  
//        //        //Send(_socket, "This is a test<EOF>");
//        //        sendDone.WaitOne();

//        //        // Receive the response from the remote device.  
//        //        Receive();
//        //        receiveDone.WaitOne();

//        //        // Write the response to the console.  
//        //        Console.WriteLine("Response received : {0}", response);

//        //        // Release the socket.  
//        //        _socket.Shutdown(SocketShutdown.Both);
//        //        _socket.Close();
//        //        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
//        //        args.buf
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Console.WriteLine(e.ToString());
//        //    }
//        //}

//        private static void ReceiveCallback(IAsyncResult ar)
//        {
//            try
//            {
//                // Retrieve the state object and the client socket   
//                // from the asynchronous state object.  
//                StateObject state = (StateObject)ar.AsyncState;
//                Socket socket = state.socket;

//                // Read data from the remote device.  
//                int bytesRead = socket.EndReceive(ar);
//                Console.WriteLine("BytesRead: " + bytesRead);

//                if (bytesRead > 0)
//                {
//                    if (state.buffer == null)
//                    {
//                        state.buffer = new byte[BitConverter.ToInt32(state.length, 0)];
//                        Console.WriteLine("Message length: " + state.buffer.Length);
//                    }
//                    else
//                        state.currentLength += bytesRead;

//                    Console.WriteLine("Available: " + socket.Available);

//                    if (state.currentLength == state.buffer.Length)
//                    {
//                        Console.WriteLine(state.currentLength);

//                        //IWaveProvider provider = new RawSourceWaveStream(
//                        //                         new MemoryStream(state.buffer), new WaveFormat(22050, 16, 1));
//                        //WaveOut _waveOut = new WaveOut();
//                        //_waveOut.Init(provider);
//                        //_waveOut.Play();
//                        //while (_waveOut.PlaybackState != PlaybackState.Stopped) { }

//                        receiveDone.Set();
//                    }
//                    else
//                    {
//                        socket.BeginReceive(state.buffer, state.currentLength, state.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
//                    }
//                }
//                else
//                {
//                    Console.WriteLine(state.currentLength);
//                    receiveDone.Set();
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        private static void ConnectCallback(IAsyncResult ar)
//        {
//            try
//            {
//                // Retrieve the socket from the state object.  
//                Socket socket = (Socket)ar.AsyncState;

//                // Complete the connection.  
//                socket.EndConnect(ar);

//                Console.WriteLine("Socket connected to {0}", socket.RemoteEndPoint.ToString());

//                // Signal that the connection has been made.  
//                connectDone.Set();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        private void SendCallback(IAsyncResult ar)
//        {
//            try
//            {
//                // Retrieve the socket from the state object.  
//                Socket client = (Socket)ar.AsyncState;

//                // Complete sending the data to the remote device.
//                int bytesSent = client.EndSend(ar);
//                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

//                // Signal that all bytes have been sent.  
//                sendDone.Set();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }
//    }
//}
