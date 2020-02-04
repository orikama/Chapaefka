using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// SocketTaskExtensions

namespace CSharpClient.TTSClient
{
    class SocketTPL
    {
        private const int SizePrefixLength = 4;
        private const int SendBufferSize = 512;
        private const int ReceiveBufferSize = 32768;

        private string _host = string.Empty;
        private int _port = -1;
        private AddressFamily _addressFamily = AddressFamily.Unspecified;
        private IPEndPoint _ipEndPoint;

        private Socket _socket = null;


        //************** properties **************************

        public bool Connected
        {
            get { return _socket is null ? false : _socket.Connected; }
        }


        //************* constructors *************************

        public SocketTPL()
        { }


        //************ public methods ************************

        // https://blog.stephencleary.com/2016/12/eliding-async-await.html
        public Task ConnectAsync(string host, int port)
        {
            bool endPointChanged = false;

            IPHostEntry ipHostInfo = null;

            if (_host != host)
            {
                _host = host;
                // NOTE: should i use GetHostEntryAsync ?
                ipHostInfo = Dns.GetHostEntry(host);

                endPointChanged = true;
            }
            if (_port != port)
            {
                _port = port;
                endPointChanged = true;
            }

            if (endPointChanged)
            {
                // NOTE: is it right to use ipHostInfo.AddressList[0] ?
                _ipEndPoint = new IPEndPoint(ipHostInfo.AddressList[0], port);
                if (_addressFamily != _ipEndPoint.AddressFamily)
                {
                    if (_socket != null)
                    {
                        // https://stackoverflow.com/questions/35229143/what-exactly-do-sockets-shutdown-disconnect-close-and-dispose-do
                        _socket.Dispose();
                    }
                    _socket = new Socket(_ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }
                _socket.ReceiveBufferSize = ReceiveBufferSize;
                _socket.SendBufferSize = SendBufferSize;
                //_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress)

                return Task.Factory.FromAsync(
                    _socket.BeginConnect(_ipEndPoint, null, null),
                    _socket.EndConnect
                    );
            }

            return Task.CompletedTask;
        }

        public Task DisconnectAsync(bool reuseSocket)
        {
            // NOTE: check for connected ?
            return Task.Factory.FromAsync(
                _socket.BeginDisconnect(reuseSocket, null, null),
                _socket.EndDisconnect
                );
        }

        public Task SendAsync(string message)
        {
            // NOTE: check for connected ?
            byte[] byteData = Encoding.UTF8.GetBytes(message);

            return Task.Factory.FromAsync(
                _socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, null, null),
                _socket.EndSend
                );
        }
    }
}
