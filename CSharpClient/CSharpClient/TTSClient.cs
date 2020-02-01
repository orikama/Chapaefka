using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        private readonly ArrayPool<byte> _wavBuffer = ArrayPool<byte>.Create(WavBufferSize, 10);
        private int _wavBufferBytesRented = 0;

        private AsyncCallback _nullCallback = (i) => { };

        private bool _disposed = false;



        public bool Connected
        {
            get { return _socket is null ? false : _socket.Connected; }
        }

        public bool BufferIsFull => _bufferIsFull;


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

        public async Task<Tuple<int, byte[]>> ReceiveWavAsync()
        {
            byte[] prefix = new byte[PrefixLength];

            int recvLen = await Task.Factory.FromAsync(
                                   (cb, s) => _socket.BeginReceive(prefix, 0, PrefixLength, SocketFlags.None, cb, s),
                                   ias => _socket.EndReceive(ias),
                                   null).ConfigureAwait(false);

            if (recvLen != PrefixLength)
                throw new ApplicationException("Failed to receive prefix");

            int contentLength = BitConverter.ToInt32(prefix, 0);
            byte[] bufferReference = _wavBuffer.Rent(contentLength + WavHeaderOffset);

            _wavBufferBytesRented += bufferReference.Length;
            if (_wavBufferBytesRented + WavSecondSize * 20 >= WavBufferSize)
                _bufferIsFull = true;

            int currentLength = 0;
            while (currentLength < contentLength)
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
