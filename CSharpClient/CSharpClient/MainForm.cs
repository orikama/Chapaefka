using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CSharpClient
{
    public partial class MainForm : Form
    {
        private TTSClient _ttsClient = null;

        public MainForm()
        {
            InitializeComponent();

            //Console.WriteLine(new Uri(host).AbsoluteUri);
            
        }

        private void btnConnectTTSServer_Click(object sender, EventArgs e)
        {
            if(tbTTSServerIP.Text != string.Empty)
            {
                btnConnectTTSServer.Enabled = false;
                tbTTSServerIP.Enabled = false;
                numericTTSServerPort.Enabled = false;
            }
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
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

        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Red, Color.SlateGray);
        }

        //IPHostEntry ipHostInfo = Dns.GetHostEntry("DESKTOP-4J6T4O5");
        //IPAddress ipAddress = ipHostInfo.AddressList[0];
        //IPEndPoint remoteEP = new IPEndPoint(ipAddress, pythonPort);

        //TTSClient ttsClient = new TTSClient("localhost", 17853);
        //ttsClient.Connect();

        //Console.Write("Connecting");
        //TTSClient.connectDone.WaitOne();

        //Console.WriteLine("\nSending");
        //ttsClient.Send("We need to build the Wall.");
        //TTSClient.sendDone.WaitOne();
        //Console.WriteLine("\nSent");

        //Console.WriteLine("\nReceiving");
        //ttsClient.Receive();
        //TTSClient.receiveDone.WaitOne();
        //Console.WriteLine("\nReceived");
    }
}
