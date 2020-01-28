using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Buffers;

//https://codereview.stackexchange.com/questions/177526/c-async-socket-wrapper

namespace CSharpClient.TTSServerSocket
{
    public class TTSClient : IDisposable
    {
        private Socket _socket = null;
        private string _ttsServerNameOrAddress = null;
        private int _ttsServerPort = -1;
        private IPEndPoint _ipEndPoint;

        private const int SendBufferSize = 512;
        private const int ReceiveBufferSize = 32768;

        private AsyncCallback _nullCallback = (i) => { };

        private bool _disposed = false;

        public bool Connected
        {
            get { return _socket is null ? false : _socket.Connected; }
        }

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

            //using (var stream = new NetworkStream(_socket))
            //{
            //    stream.write
            //    await stream.WriteAsync(buffer, offset, count);
            //}
        }

        public async Task<int> ReceiveAsync(byte[] buffer, int offset, int count)
        {
            using (var stream = new NetworkStream(_socket))
            {
                return await stream.ReadAsync(buffer, offset, count).ConfigureAwait(false);
            }
        }

        public async Task DisconnectAsync()
        {
            _socket.Shutdown(SocketShutdown.Both);
            await Task.Factory.FromAsync(_socket.BeginDisconnect, _socket.EndDisconnect, true, null).ConfigureAwait(false);
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

    struct WavHeader
    {
        UInt32 ChunkId;         // "RIFF" - 0x52494646
        UInt32 ChunkSize;       // fileSize minus size of ChunkId and ChunkSize => fileSize - 8 
        UInt32 Format;          // "WAVE" - 0x57415645
        UInt32 Subchunk1Id;     // "fmt " - 0x666d7420
        UInt32 Subchunk1Size;   // 16 for PCM format
        UInt16 AudioFormat;     // 1 for PCM format
        UInt16 NumChannels;     // mono=1, stereo=2 etc.
        UInt32 SampleRate;      // 22050Hz, 44100Hz etc.
        UInt32 ByteRate;        // sampleRate * numChannels * bitsPerSample/8
        UInt16 BlockAlign;      // numChannels * bitsPerSample/8
        UInt16 BitsPerSample;   // sound depth - 8 bit, 16 bit, etc.
        UInt32 Subchunk2Id;     // "data" - 0x64617461
        UInt32 Subchunk2Size;   // numSamples * numChannels * bitsPerSample/8
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
