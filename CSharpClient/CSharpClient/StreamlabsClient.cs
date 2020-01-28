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
        private decimal _minimumDonation;

        private readonly IO.Options _ioOptions = null;
        private SocketIO _socketIO = null;
        private bool _connected = false;

        private Queue<DonationEventArgs> _donationQueue = new Queue<DonationEventArgs>(10);

        public event ConnectionEventHandler OnConnect = null;
        public event ConnectionEventHandler OnDisconnect = null;
        public event DataEventHandler OnDonation = null;

        public bool Connected => _connected;

        public decimal MinimumDonation
        {
            get { return _minimumDonation; }
            set { _minimumDonation = value; }
        }

        public Queue<DonationEventArgs> DonationQueue => _donationQueue;

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
            if(_token != token)
            {
                _token = token;
                _ioOptions.Query["token"] = token;
                _socketIO = IO.Socket(s_uri, _ioOptions);

                _socketIO.On(SocketIO.EVENT_CONNECT, () => ConnectedCallback());
                _socketIO.On(SocketIO.EVENT_DISCONNECT, () => DisconnectedCallback());
                _socketIO.On("event", (data) => DataReceivedCallback(data));
            }
        }

        public void Connect() => _socketIO.Connect();
        public void Disconnect() => _socketIO.Disconnect();

        private void ConnectedCallback()
        {
            _connected = true;

            if (OnConnect != null)
            {
                BaseEventArgs args = new BaseEventArgs();
                OnConnect(args);
            }
        }

        private void DisconnectedCallback()
        {
            _connected = false;

            if (OnDisconnect != null)
            {
                BaseEventArgs args = new BaseEventArgs();
                OnDisconnect(args);
            }
        }

        private void DataReceivedCallback(object data)
        {
            JObject jData = (JObject)data;

            bool evt_type_exists = jData.TryGetValue("type", out JToken evt_type);

            if (evt_type_exists && jData.Value<string>("type") == "donation")
            {
                JToken dontaionMessage = jData["message"][0];

                if (OnDonation != null && dontaionMessage["amount"].ToObject<decimal>() >= _minimumDonation)
                {
                    DonationEventArgs args = new DonationEventArgs()
                    {
                        From = dontaionMessage["from"].ToString(),
                        Message = dontaionMessage["message"].ToString(),
                        Amount = dontaionMessage["formatted_amount"].ToString()

                    };
                    _donationQueue.Enqueue(args);

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
