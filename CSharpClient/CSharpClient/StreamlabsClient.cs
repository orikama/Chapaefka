using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using ImmutableList = Quobject.Collections.Immutable.ImmutableList;
using SocketIO = Quobject.SocketIoClientDotNet.Client.Socket;

namespace CSharpClient.StreamlabsSocket
{
    public class StreamlabsClient
    {
        private const string _host = "https://sockets.streamlabs.com";
        private static readonly Uri s_uri = new Uri(_host);

        private string _token = null;
        private double _minimumDonation;

        private readonly IO.Options _ioOptions = null;
        private SocketIO _socketIO = null;

        public event ConnectionEventHandler OnConnect = null;
        public event ConnectionEventHandler OnDisconnect = null;
        public event DataEventHandler OnDonation = null;

        public double MinimumDonation
        {
            get { return _minimumDonation; }
            set { _minimumDonation = value; }
        }

        public StreamlabsClient()
        {
            _ioOptions = new IO.Options
            {
                Host = _host,
                Transports = ImmutableList.Create("websocket"),
                AutoConnect = false,
                Query = new Dictionary<string, string>
                {
                    { "token", string.Empty }
                }
            };
        }

        public void Init(string token)
        {
            _token = token;
            _ioOptions.Query["token"] = _token;
            _socketIO = IO.Socket(s_uri, _ioOptions);

            _socketIO.On(SocketIO.EVENT_CONNECT, () => Connected());
            _socketIO.On(SocketIO.EVENT_DISCONNECT, () => Disconnected());
            _socketIO.On("event", (data) => DataReceived(data));
        }

        public void Connect() => _socketIO.Connect();
        public void Disconnect() => _socketIO.Disconnect();

        private void Connected()
        {
            if (OnConnect != null)
            {
                BaseEventArgs args = new BaseEventArgs();

                OnConnect(args);
            }
        }

        private void Disconnected()
        {
            if (OnDisconnect != null)
            {
                BaseEventArgs args = new BaseEventArgs();

                OnDisconnect(args);
            }
        }

        private void DataReceived(object data)
        {
            JObject jData = (JObject)data;

            bool evt_type_exists = jData.TryGetValue("type", out JToken evt_type);

            if (evt_type_exists && jData.Value<string>("type") == "donation")
            {
                JToken dontaionMessage = jData["message"][0];

                if (OnDonation != null &&
                    dontaionMessage["amount"].ToObject<double>() >= _minimumDonation)
                {
                    DonationEventArgs args = new DonationEventArgs()
                    {
                        From = dontaionMessage["from"].ToString(),
                        Message = dontaionMessage["message"].ToString(),
                        Amount = dontaionMessage["formatted_amount"].ToString()

                    };

                    OnDonation(args);
                }
            }
        }

    }

    public delegate void ConnectionEventHandler(BaseEventArgs e);
    public delegate void DataEventHandler(DonationEventArgs e);

    public class BaseEventArgs : EventArgs
    {
        public string Time = DateTime.Now.ToString("HH:mm:ss");
    }

    public class DonationEventArgs : BaseEventArgs
    {
        public string From { get; set; }
        public string Message { get; set; }
        public string Amount { get; set; }
    }

}
