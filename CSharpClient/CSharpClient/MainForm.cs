using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Streamlabs = CSharpClient.StreamlabsSocket;
using TTS = CSharpClient.TTSServerSocket;
using JSettings = CSharpClient.AppSettings;

namespace CSharpClient
{
    public partial class MainForm : Form
    {
        private TTS.TTSClient _ttsClient = null;
        private Streamlabs.StreamlabsClient _streamlabsClient = null;
        private JSettings.SettingsLoader _jsettings = null;

        public MainForm()
        {
            InitializeComponent();

            _ttsClient = new TTS.TTSClient();
            _jsettings = new JSettings.SettingsLoader();

            _streamlabsClient = new Streamlabs.StreamlabsClient();
            _streamlabsClient.OnConnect += StreamlabsOnConnect;
            _streamlabsClient.OnDisconnect += StreamlabsOnDisconnect;
            _streamlabsClient.OnDonation += StreamlabsOnDonation;
        }

        #region Streamlabs

        private void StreamlabsOnConnect(Streamlabs.BaseEventArgs e)
        {
            btnDisconnectStreamlabs.InvokeIfRequired(btn => { btn.Enabled = true; });
            statusStrip.InvokeIfRequired(ss => { ss.Items["statusLblStreamlabs"].Image = Properties.Resources.connectedIcon; });
        }

        private void StreamlabsOnDisconnect(Streamlabs.BaseEventArgs e)
        {
            btnConnectStreamlabs.InvokeIfRequired(btn => { btn.Enabled = true; });
            statusStrip.InvokeIfRequired(ss => { ss.Items["statusLblStreamlabs"].Image = Properties.Resources.disconnectedIcon; });
        }

        private void StreamlabsOnDonation(Streamlabs.DonationEventArgs e)
        {
            tbDonationMsg.InvokeIfRequired(tb => { tb.Text = $"{e.Time} {e.From} {e.Amount}\r\n{e.Message}"; });
        }

        private void StreamlabsConnect(string token)
        {
            btnConnectStreamlabs.Enabled = false;
            tbStreamlabsToken.Enabled = false;
            _streamlabsClient.Init(token);
            _streamlabsClient.Connect();
        }

        private void btnConnectStreamlabs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbStreamlabsToken.Text) == false)
            {
                StreamlabsConnect(tbStreamlabsToken.Text);
            }
        }

        private void btnDisconnectStreamlabs_Click(object sender, EventArgs e)
        {
            btnDisconnectStreamlabs.Enabled = false;
            tbStreamlabsToken.Enabled = true;
            _streamlabsClient.Disconnect();
        }

        #endregion Streamlabs

        #region TTSServer

        private async Task TTSServerConnect(string host, int port)
        {
            btnConnectTTSServer.Enabled = false;
            tbTTSServerIP.Enabled = false;
            tbTTSServerPort.Enabled = false;

            await _ttsClient.ConnectAsync(host, port).ConfigureAwait(true);

            btnDisconnectTTSServer.Enabled = true;
            statusLblTTSServer.Image = Properties.Resources.connectedIcon;
            btnSpeak.Enabled = true;
        }

        private async void btnConnectTTSServer_Click(object sender, EventArgs e)
        {
            int port = int.Parse(tbTTSServerPort.Text);

            if (string.IsNullOrEmpty(tbTTSServerIP.Text) == false && port > 1000 && port < 65536)
            {
                await TTSServerConnect(tbTTSServerIP.Text, port).ConfigureAwait(true);
            }
        }

        private async void btnDisconnectTTSServer_Click(object sender, EventArgs e)
        {
            btnDisconnectTTSServer.Enabled = false;

            await _ttsClient.DisconnectAsync().ConfigureAwait(true);

            tbTTSServerIP.Enabled = true;
            tbTTSServerPort.Enabled = true;
            btnConnectTTSServer.Enabled = true;
            statusLblTTSServer.Image = Properties.Resources.disconnectedIcon;
        }

        private async void btnSpeak_Click(object sender, EventArgs e)
        {
            btnSpeak.Enabled = false;
            tbDonationMsg.Enabled = false;

            await _ttsClient.SendAsync(tbDonationMsg.Text).ConfigureAwait(true);

            btnSpeak.Enabled = true;
            tbDonationMsg.Enabled = true;
        }

        #endregion  TTSServer

        private void numericMinimum_ValueChanged(object sender, EventArgs e)
        {
            _streamlabsClient.MinimumDonation = numericMinimum.Value;
        }

        private void cbHideIPandPort_CheckedChanged(object sender, EventArgs e)
        {
            tbTTSServerIP.UseSystemPasswordChar ^= true;
            tbTTSServerPort.UseSystemPasswordChar ^= true;
        }

        private void cbDenoiser_CheckedChanged(object sender, EventArgs e)
        {
            lblDenoiserStrength.Visible ^= true;
            tbDenoiserStrength.Visible ^= true;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (_jsettings.CanLoadSettings)
            {
                _jsettings.LoadSettings();
                JSettings.Settings settings = _jsettings.Settings;

                tbTTSServerIP.Text = settings.NameOrIP;
                tbTTSServerPort.Text = settings.Port.ToString();
                cbHideIPandPort.Checked = settings.HideIPandPort;
                tbStreamlabsToken.Text = settings.Token;
                numericMinimum.Value = settings.MinimumDonation;
                cbDenoiser.Checked = settings.UseDenoiser;
                tbDenoiserStrength.Text = settings.DenoiserStrength;
                tbSigma.Text = settings.Sigma;

                StreamlabsConnect(settings.Token);
                await TTSServerConnect(settings.NameOrIP, settings.Port).ConfigureAwait(true);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_ttsClient != null && _ttsClient.Connected)
            {
                Task task = _ttsClient.DisconnectAsync();

                JSettings.Settings settings = _jsettings.Settings;
                settings.NameOrIP = tbTTSServerIP.Text;
                settings.Port = ushort.Parse(tbTTSServerPort.Text);
                settings.HideIPandPort = cbHideIPandPort.Checked;
                settings.Token = tbStreamlabsToken.Text;
                settings.MinimumDonation = numericMinimum.Value;
                settings.UseDenoiser = cbDenoiser.Checked;
                settings.DenoiserStrength = tbDenoiserStrength.Text;
                settings.Sigma = tbSigma.Text;

                _jsettings.SaveSettings();
                task.Wait();
            }
        }

        // https://stackoverflow.com/questions/76455/how-do-you-change-the-color-of-the-border-on-a-group-box
        // https://stackoverflow.com/questions/34562088/custom-groupbox-with-custom-textcolor-bordercolor-and-transparent-backcolor
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Red, Color.Black);
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                using (Brush textBrush = new SolidBrush(textColor))
                using (Brush borderBrush = new SolidBrush(borderColor))
                using (Pen borderPen = new Pen(borderBrush))
                {
                    SizeF strSize = g.MeasureString(box.Text, box.Font);
                    Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                                   box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                                   box.ClientRectangle.Width - 1,
                                                   box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                    // Clear text and border
                    g.Clear(this.BackColor);

                    // Draw text
                    g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                    // Drawing Border
                    //Left
                    g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                    //Right
                    g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                    //Bottom
                    g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                    //Top1
                    g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                    //Top2
                    g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
                }

            }
        }

        ~MainForm()
        {
            _ttsClient?.Dispose();
            base.Dispose();
        }



        //IPHostEntry ipHostInfo = Dns.GetHostEntry("DESKTOP-4J6T4O5");
        //IPAddress ipAddress = ipHostInfo.AddressList[0];
        //IPEndPoint remoteEP = new IPEndPoint(ipAddress, pythonPort);

        //TTSClient ttsClient = new TTSClient("localhost", 17853);
        //ttsClient.Connect();
    }

    // https://stackoverflow.com/questions/2367718/automating-the-invokerequired-code-pattern/12179408
    public static class ControlHelpers
    {
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            //Contract.Requires<ArgumentNullException>(action != null, "Action cannot be null.");
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => action(control)), null);
            }
            else
            {
                action(control);
            }
        }
    }
}
