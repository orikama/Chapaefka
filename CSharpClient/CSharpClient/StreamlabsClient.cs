using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quobject.SocketIoClientDotNet.Client;
using SocketIO = Quobject.SocketIoClientDotNet.Client.Socket;
using ImmutableList = Quobject.Collections.Immutable.ImmutableList;

namespace CSharpClient
{
    class StreamlabsClient
    {
        private const string _host = "https://sockets.streamlabs.com";
        private readonly static Uri _uri = new Uri(_host);

        private string _token = null;

        private readonly IO.Options _ioOptions = null;
        private readonly SocketIO _socketIO = null;

        StreamlabsClient(string token)
        {
            _token = token;
            _ioOptions = new IO.Options
            {
                Host = _host,
                Transports = ImmutableList.Create("websocket"),
                AutoConnect = false,
                Query = new Dictionary<string, string>
                {
                    { "token", token }
                }
            };
            
            _socketIO = IO.Socket(_uri, _ioOptions);

            //_socketIO.On(SocketIO.EVENT_CONNECT, () => SocketConnected());
            //_socketIO.On(SocketIO.EVENT_DISCONNECT, () => SocketDisconnected());
            //_socketIO.On("event", (data) => SocketEvent(data));
        }

        public void Connect()
        {
            _socketIO.Connect();
        }

    }
}
