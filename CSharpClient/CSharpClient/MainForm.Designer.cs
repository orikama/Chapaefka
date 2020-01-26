namespace CSharpClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbDonationMsg = new System.Windows.Forms.TextBox();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnSkpeak = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTTS = new System.Windows.Forms.TabPage();
            this.lblDonationMsg = new System.Windows.Forms.Label();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.gbAudio = new System.Windows.Forms.GroupBox();
            this.tbSigma = new System.Windows.Forms.TextBox();
            this.lblSigma = new System.Windows.Forms.Label();
            this.tbDenoiserStrength = new System.Windows.Forms.TextBox();
            this.lblDenoiserStrength = new System.Windows.Forms.Label();
            this.cbDenoiser = new System.Windows.Forms.CheckBox();
            this.gbTTSServer = new System.Windows.Forms.GroupBox();
            this.lblTTSServerIP = new System.Windows.Forms.Label();
            this.tbTTSServerIP = new System.Windows.Forms.TextBox();
            this.btnDisconnectTTSServer = new System.Windows.Forms.Button();
            this.btnConnectTTSServer = new System.Windows.Forms.Button();
            this.lblTTSServerPort = new System.Windows.Forms.Label();
            this.gbStreamlabs = new System.Windows.Forms.GroupBox();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.numericMinimum = new System.Windows.Forms.NumericUpDown();
            this.lblStreamlabsToken = new System.Windows.Forms.Label();
            this.tbStreamlabsToken = new System.Windows.Forms.TextBox();
            this.btnDisconnectStreamlabs = new System.Windows.Forms.Button();
            this.btnConnectStreamlabs = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLblStreamlabs = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLblTTSServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.cbHideIPandPort = new System.Windows.Forms.CheckBox();
            this.tbTTSServerPort = new System.Windows.Forms.TextBox();
            this.btnAudioApply = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabTTS.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.gbAudio.SuspendLayout();
            this.gbTTSServer.SuspendLayout();
            this.gbStreamlabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMinimum)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDonationMsg
            // 
            this.tbDonationMsg.Location = new System.Drawing.Point(6, 41);
            this.tbDonationMsg.Multiline = true;
            this.tbDonationMsg.Name = "tbDonationMsg";
            this.tbDonationMsg.Size = new System.Drawing.Size(514, 194);
            this.tbDonationMsg.TabIndex = 0;
            // 
            // btnSkip
            // 
            this.btnSkip.Enabled = false;
            this.btnSkip.Location = new System.Drawing.Point(352, 291);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 1;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // btnSkpeak
            // 
            this.btnSkpeak.Enabled = false;
            this.btnSkpeak.Location = new System.Drawing.Point(433, 291);
            this.btnSkpeak.Name = "btnSkpeak";
            this.btnSkpeak.Size = new System.Drawing.Size(75, 23);
            this.btnSkpeak.TabIndex = 2;
            this.btnSkpeak.Text = "Speak";
            this.btnSkpeak.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabTTS);
            this.tabControl.Controls.Add(this.tabOptions);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl.HotTrack = true;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(536, 370);
            this.tabControl.TabIndex = 3;
            // 
            // tabTTS
            // 
            this.tabTTS.Controls.Add(this.lblDonationMsg);
            this.tabTTS.Controls.Add(this.btnSkpeak);
            this.tabTTS.Controls.Add(this.tbDonationMsg);
            this.tabTTS.Controls.Add(this.btnSkip);
            this.tabTTS.Location = new System.Drawing.Point(4, 24);
            this.tabTTS.Name = "tabTTS";
            this.tabTTS.Padding = new System.Windows.Forms.Padding(3);
            this.tabTTS.Size = new System.Drawing.Size(528, 342);
            this.tabTTS.TabIndex = 0;
            this.tabTTS.Text = "TTS";
            this.tabTTS.UseVisualStyleBackColor = true;
            // 
            // lblDonationMsg
            // 
            this.lblDonationMsg.AutoSize = true;
            this.lblDonationMsg.Location = new System.Drawing.Point(8, 13);
            this.lblDonationMsg.Name = "lblDonationMsg";
            this.lblDonationMsg.Size = new System.Drawing.Size(111, 15);
            this.lblDonationMsg.TabIndex = 3;
            this.lblDonationMsg.Text = "Donation message";
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.gbAudio);
            this.tabOptions.Controls.Add(this.gbTTSServer);
            this.tabOptions.Controls.Add(this.gbStreamlabs);
            this.tabOptions.Location = new System.Drawing.Point(4, 24);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(528, 342);
            this.tabOptions.TabIndex = 1;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // gbAudio
            // 
            this.gbAudio.Controls.Add(this.btnAudioApply);
            this.gbAudio.Controls.Add(this.tbSigma);
            this.gbAudio.Controls.Add(this.lblSigma);
            this.gbAudio.Controls.Add(this.lblDenoiserStrength);
            this.gbAudio.Controls.Add(this.cbDenoiser);
            this.gbAudio.Controls.Add(this.tbDenoiserStrength);
            this.gbAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbAudio.Location = new System.Drawing.Point(3, 200);
            this.gbAudio.Name = "gbAudio";
            this.gbAudio.Size = new System.Drawing.Size(214, 111);
            this.gbAudio.TabIndex = 13;
            this.gbAudio.TabStop = false;
            this.gbAudio.Text = "Audio";
            this.gbAudio.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox_Paint);
            // 
            // tbSigma
            // 
            this.tbSigma.Location = new System.Drawing.Point(68, 48);
            this.tbSigma.MaxLength = 5;
            this.tbSigma.Name = "tbSigma";
            this.tbSigma.Size = new System.Drawing.Size(53, 21);
            this.tbSigma.TabIndex = 5;
            this.tbSigma.Text = "0.800";
            // 
            // lblSigma
            // 
            this.lblSigma.AutoSize = true;
            this.lblSigma.Location = new System.Drawing.Point(19, 51);
            this.lblSigma.Name = "lblSigma";
            this.lblSigma.Size = new System.Drawing.Size(43, 15);
            this.lblSigma.TabIndex = 4;
            this.lblSigma.Text = "Sigma";
            // 
            // tbDenoiserStrength
            // 
            this.tbDenoiserStrength.Location = new System.Drawing.Point(156, 18);
            this.tbDenoiserStrength.MaxLength = 4;
            this.tbDenoiserStrength.Name = "tbDenoiserStrength";
            this.tbDenoiserStrength.Size = new System.Drawing.Size(37, 21);
            this.tbDenoiserStrength.TabIndex = 3;
            this.tbDenoiserStrength.Text = "0.01";
            this.tbDenoiserStrength.Visible = false;
            // 
            // lblDenoiserStrength
            // 
            this.lblDenoiserStrength.AutoSize = true;
            this.lblDenoiserStrength.Location = new System.Drawing.Point(97, 21);
            this.lblDenoiserStrength.Name = "lblDenoiserStrength";
            this.lblDenoiserStrength.Size = new System.Drawing.Size(53, 15);
            this.lblDenoiserStrength.TabIndex = 2;
            this.lblDenoiserStrength.Text = "Strength";
            this.lblDenoiserStrength.Visible = false;
            // 
            // cbDenoiser
            // 
            this.cbDenoiser.AutoSize = true;
            this.cbDenoiser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbDenoiser.Location = new System.Drawing.Point(7, 20);
            this.cbDenoiser.Name = "cbDenoiser";
            this.cbDenoiser.Size = new System.Drawing.Size(76, 19);
            this.cbDenoiser.TabIndex = 1;
            this.cbDenoiser.Text = "Denoiser";
            this.cbDenoiser.UseVisualStyleBackColor = true;
            this.cbDenoiser.CheckedChanged += new System.EventHandler(this.cbDenoiser_CheckedChanged);
            // 
            // gbTTSServer
            // 
            this.gbTTSServer.Controls.Add(this.tbTTSServerPort);
            this.gbTTSServer.Controls.Add(this.cbHideIPandPort);
            this.gbTTSServer.Controls.Add(this.lblTTSServerIP);
            this.gbTTSServer.Controls.Add(this.tbTTSServerIP);
            this.gbTTSServer.Controls.Add(this.btnDisconnectTTSServer);
            this.gbTTSServer.Controls.Add(this.btnConnectTTSServer);
            this.gbTTSServer.Controls.Add(this.lblTTSServerPort);
            this.gbTTSServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTTSServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbTTSServer.Location = new System.Drawing.Point(3, 3);
            this.gbTTSServer.Name = "gbTTSServer";
            this.gbTTSServer.Size = new System.Drawing.Size(522, 89);
            this.gbTTSServer.TabIndex = 11;
            this.gbTTSServer.TabStop = false;
            this.gbTTSServer.Text = "TTS Server";
            this.gbTTSServer.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox_Paint);
            // 
            // lblTTSServerIP
            // 
            this.lblTTSServerIP.AutoSize = true;
            this.lblTTSServerIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTTSServerIP.Location = new System.Drawing.Point(6, 25);
            this.lblTTSServerIP.Name = "lblTTSServerIP";
            this.lblTTSServerIP.Size = new System.Drawing.Size(88, 15);
            this.lblTTSServerIP.TabIndex = 7;
            this.lblTTSServerIP.Text = "IP or PC Name";
            // 
            // tbTTSServerIP
            // 
            this.tbTTSServerIP.Location = new System.Drawing.Point(100, 22);
            this.tbTTSServerIP.MaxLength = 1000;
            this.tbTTSServerIP.Name = "tbTTSServerIP";
            this.tbTTSServerIP.Size = new System.Drawing.Size(163, 21);
            this.tbTTSServerIP.TabIndex = 6;
            // 
            // btnDisconnectTTSServer
            // 
            this.btnDisconnectTTSServer.Enabled = false;
            this.btnDisconnectTTSServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDisconnectTTSServer.Location = new System.Drawing.Point(401, 49);
            this.btnDisconnectTTSServer.Name = "btnDisconnectTTSServer";
            this.btnDisconnectTTSServer.Size = new System.Drawing.Size(84, 23);
            this.btnDisconnectTTSServer.TabIndex = 5;
            this.btnDisconnectTTSServer.Text = "Disconnect";
            this.btnDisconnectTTSServer.UseVisualStyleBackColor = true;
            // 
            // btnConnectTTSServer
            // 
            this.btnConnectTTSServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConnectTTSServer.Location = new System.Drawing.Point(401, 20);
            this.btnConnectTTSServer.Name = "btnConnectTTSServer";
            this.btnConnectTTSServer.Size = new System.Drawing.Size(84, 23);
            this.btnConnectTTSServer.TabIndex = 4;
            this.btnConnectTTSServer.Text = "Connect";
            this.btnConnectTTSServer.UseVisualStyleBackColor = true;
            this.btnConnectTTSServer.Click += new System.EventHandler(this.btnConnectTTSServer_Click);
            // 
            // lblTTSServerPort
            // 
            this.lblTTSServerPort.AutoSize = true;
            this.lblTTSServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTTSServerPort.Location = new System.Drawing.Point(275, 25);
            this.lblTTSServerPort.Name = "lblTTSServerPort";
            this.lblTTSServerPort.Size = new System.Drawing.Size(29, 15);
            this.lblTTSServerPort.TabIndex = 8;
            this.lblTTSServerPort.Text = "Port";
            // 
            // gbStreamlabs
            // 
            this.gbStreamlabs.BackColor = System.Drawing.Color.Transparent;
            this.gbStreamlabs.Controls.Add(this.lblMinimum);
            this.gbStreamlabs.Controls.Add(this.numericMinimum);
            this.gbStreamlabs.Controls.Add(this.lblStreamlabsToken);
            this.gbStreamlabs.Controls.Add(this.tbStreamlabsToken);
            this.gbStreamlabs.Controls.Add(this.btnConnectStreamlabs);
            this.gbStreamlabs.Controls.Add(this.btnDisconnectStreamlabs);
            this.gbStreamlabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbStreamlabs.Location = new System.Drawing.Point(3, 98);
            this.gbStreamlabs.Name = "gbStreamlabs";
            this.gbStreamlabs.Size = new System.Drawing.Size(522, 96);
            this.gbStreamlabs.TabIndex = 12;
            this.gbStreamlabs.TabStop = false;
            this.gbStreamlabs.Text = "Streamlabs";
            this.gbStreamlabs.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox_Paint);
            // 
            // lblMinimum
            // 
            this.lblMinimum.AutoSize = true;
            this.lblMinimum.Location = new System.Drawing.Point(6, 71);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(113, 15);
            this.lblMinimum.TabIndex = 5;
            this.lblMinimum.Text = "Minimum Donation";
            // 
            // numericMinimum
            // 
            this.numericMinimum.Location = new System.Drawing.Point(125, 69);
            this.numericMinimum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericMinimum.Name = "numericMinimum";
            this.numericMinimum.Size = new System.Drawing.Size(50, 21);
            this.numericMinimum.TabIndex = 4;
            this.numericMinimum.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.numericMinimum.ValueChanged += new System.EventHandler(this.numericMinimum_ValueChanged);
            // 
            // lblStreamlabsToken
            // 
            this.lblStreamlabsToken.AutoSize = true;
            this.lblStreamlabsToken.Location = new System.Drawing.Point(5, 22);
            this.lblStreamlabsToken.Name = "lblStreamlabsToken";
            this.lblStreamlabsToken.Size = new System.Drawing.Size(367, 15);
            this.lblStreamlabsToken.TabIndex = 1;
            this.lblStreamlabsToken.Text = "Streamlabs Socket API Token (Settings->API Settings->API tokens)";
            // 
            // tbStreamlabsToken
            // 
            this.tbStreamlabsToken.Location = new System.Drawing.Point(8, 42);
            this.tbStreamlabsToken.Name = "tbStreamlabsToken";
            this.tbStreamlabsToken.Size = new System.Drawing.Size(354, 21);
            this.tbStreamlabsToken.TabIndex = 0;
            this.tbStreamlabsToken.Text = resources.GetString("tbStreamlabsToken.Text");
            this.tbStreamlabsToken.UseSystemPasswordChar = true;
            // 
            // btnDisconnectStreamlabs
            // 
            this.btnDisconnectStreamlabs.Enabled = false;
            this.btnDisconnectStreamlabs.Location = new System.Drawing.Point(401, 51);
            this.btnDisconnectStreamlabs.Name = "btnDisconnectStreamlabs";
            this.btnDisconnectStreamlabs.Size = new System.Drawing.Size(84, 23);
            this.btnDisconnectStreamlabs.TabIndex = 3;
            this.btnDisconnectStreamlabs.Text = "Disconnect";
            this.btnDisconnectStreamlabs.UseVisualStyleBackColor = true;
            this.btnDisconnectStreamlabs.Click += new System.EventHandler(this.btnDisconnectStreamlabs_Click);
            // 
            // btnConnectStreamlabs
            // 
            this.btnConnectStreamlabs.Location = new System.Drawing.Point(401, 22);
            this.btnConnectStreamlabs.Name = "btnConnectStreamlabs";
            this.btnConnectStreamlabs.Size = new System.Drawing.Size(84, 23);
            this.btnConnectStreamlabs.TabIndex = 2;
            this.btnConnectStreamlabs.Text = "Connect";
            this.btnConnectStreamlabs.UseVisualStyleBackColor = true;
            this.btnConnectStreamlabs.Click += new System.EventHandler(this.btnConnectStreamlabs_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLblStreamlabs,
            this.statusLblTTSServer});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 348);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(536, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLblStreamlabs
            // 
            this.statusLblStreamlabs.Image = global::CSharpClient.Properties.Resources.disconnectedIcon;
            this.statusLblStreamlabs.Name = "statusLblStreamlabs";
            this.statusLblStreamlabs.Size = new System.Drawing.Size(81, 17);
            this.statusLblStreamlabs.Text = "Streamlabs";
            // 
            // statusLblTTSServer
            // 
            this.statusLblTTSServer.Image = global::CSharpClient.Properties.Resources.disconnectedIcon;
            this.statusLblTTSServer.Name = "statusLblTTSServer";
            this.statusLblTTSServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusLblTTSServer.Size = new System.Drawing.Size(76, 17);
            this.statusLblTTSServer.Text = "TTS Server";
            // 
            // cbHideIPandPort
            // 
            this.cbHideIPandPort.AutoSize = true;
            this.cbHideIPandPort.Location = new System.Drawing.Point(6, 57);
            this.cbHideIPandPort.Name = "cbHideIPandPort";
            this.cbHideIPandPort.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbHideIPandPort.Size = new System.Drawing.Size(115, 19);
            this.cbHideIPandPort.TabIndex = 10;
            this.cbHideIPandPort.Text = "Hide IP and Port";
            this.cbHideIPandPort.UseVisualStyleBackColor = true;
            this.cbHideIPandPort.CheckedChanged += new System.EventHandler(this.cbHideIPandPort_CheckedChanged);
            // 
            // tbTTSServerPort
            // 
            this.tbTTSServerPort.Location = new System.Drawing.Point(310, 22);
            this.tbTTSServerPort.MaxLength = 5;
            this.tbTTSServerPort.Name = "tbTTSServerPort";
            this.tbTTSServerPort.Size = new System.Drawing.Size(52, 21);
            this.tbTTSServerPort.TabIndex = 11;
            // 
            // btnAudioApply
            // 
            this.btnAudioApply.Location = new System.Drawing.Point(68, 75);
            this.btnAudioApply.Name = "btnAudioApply";
            this.btnAudioApply.Size = new System.Drawing.Size(75, 23);
            this.btnAudioApply.TabIndex = 6;
            this.btnAudioApply.Text = "Apply";
            this.btnAudioApply.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 370);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Chapaefka";
            this.tabControl.ResumeLayout(false);
            this.tabTTS.ResumeLayout(false);
            this.tabTTS.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.gbAudio.ResumeLayout(false);
            this.gbAudio.PerformLayout();
            this.gbTTSServer.ResumeLayout(false);
            this.gbTTSServer.PerformLayout();
            this.gbStreamlabs.ResumeLayout(false);
            this.gbStreamlabs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMinimum)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDonationMsg;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnSkpeak;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTTS;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.TextBox tbStreamlabsToken;
        private System.Windows.Forms.Label lblDonationMsg;
        private System.Windows.Forms.Button btnDisconnectStreamlabs;
        private System.Windows.Forms.Button btnConnectStreamlabs;
        private System.Windows.Forms.Label lblStreamlabsToken;
        private System.Windows.Forms.Button btnConnectTTSServer;
        private System.Windows.Forms.Label lblTTSServerPort;
        private System.Windows.Forms.Label lblTTSServerIP;
        private System.Windows.Forms.TextBox tbTTSServerIP;
        private System.Windows.Forms.Button btnDisconnectTTSServer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLblStreamlabs;
        private System.Windows.Forms.ToolStripStatusLabel statusLblTTSServer;
        private System.Windows.Forms.GroupBox gbStreamlabs;
        private System.Windows.Forms.GroupBox gbTTSServer;
        private System.Windows.Forms.GroupBox gbAudio;
        private System.Windows.Forms.CheckBox cbDenoiser;
        private System.Windows.Forms.Label lblSigma;
        private System.Windows.Forms.TextBox tbDenoiserStrength;
        private System.Windows.Forms.Label lblDenoiserStrength;
        private System.Windows.Forms.TextBox tbSigma;
        private System.Windows.Forms.Label lblMinimum;
        private System.Windows.Forms.NumericUpDown numericMinimum;
        private System.Windows.Forms.CheckBox cbHideIPandPort;
        private System.Windows.Forms.TextBox tbTTSServerPort;
        private System.Windows.Forms.Button btnAudioApply;
    }
}