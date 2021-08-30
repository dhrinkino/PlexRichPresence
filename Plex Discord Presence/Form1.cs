using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordRPC;
using Newtonsoft.Json;
using System.Net;
using System.Xml;
using System.IO;

namespace Plex_Discord_Presence
{
    public partial class Form1 : Form
    {
        private DiscordRpcClient client;
        private int tick = 0;
        private bool status = false;
        private dynamic now = DateTime.UtcNow;
        private bool detected = false;
        public Form1()
        {
            InitializeComponent();
            label5.Text = null;
            notifyIcon1.Icon = SystemIcons.Application;
            notifyIcon1.BalloonTipTitle = "Rich Presence for Plex";
            notifyIcon1.BalloonTipText = "Rich presence is still running";

            // Auth sequence
            PlexAuth PlexController = new PlexAuth();
            PlexController.Set("14212628094769269459", "Discord Rich Presence For Plex");
            if (!PlexController.LoadPin("plex_credentials.txt"))
            {
                PlexController.Generate();
                PlexController.Token();
                PlexController.SavePin("plex_credentials.txt");
            } else
            {
                if (!PlexController.ValidToken())
                {
                    PlexController.Token();
                    PlexController.SavePin("plex_credentials.txt");
                }
            }
            Properties.Settings.Default.PlexToken = PlexController.CurrentToken();
            Properties.Settings.Default.PlexDirect = PlexController.GetServer();

            if (Properties.Settings.Default.PlexToken != null && Properties.Settings.Default.PlexDirect != null)
            {
                DiscordInitialize();
                timer1.Start();
                status = !status;
                button1.Text = "Stop";
            } else
            {
                MessageBox.Show("Failed to fetch Token or Server URI");
            }

        }


        public void DiscordInitialize()
        {
            client = new DiscordRpcClient("881197661611495435"); // default plex app
            client.Initialize();
        }

        private bool GetPlexData()
        {
            string response = null;
            try {
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                response = new WebClient().DownloadString(Properties.Settings.Default.PlexDirect +"/status/sessions?X-Plex-Token=" + Properties.Settings.Default.PlexToken);
            } catch
            {
                timer1.Stop();
                MessageBox.Show("Cannot download to the Plex API");
                status = !status;
                button1.Text = "Run";
                label1.Text = "Offline";
                return false;
            }; 

                XmlDocument PlexXml = new XmlDocument();
                PlexXml.LoadXml(response);
                string jsonText = JsonConvert.SerializeXmlNode(PlexXml);
                dynamic stuff = JsonConvert.DeserializeObject(jsonText);
                if (stuff["MediaContainer"]["@size"] != 0)
                {

                    string text = null;
                    string text_second = null;
                    if (stuff["MediaContainer"]["@size"] > 1)
                    {
                        if (detected == false)
                        {
                            // setting new datetime
                            now = DateTime.UtcNow;
                            detected = !detected;
                        }

                        if (stuff["MediaContainer"]["Video"][0]["@type"] == "movie")
                        {
                            text = "▶ " + stuff["MediaContainer"]["Video"][0]["@title"];

                        }
                        else
                        {
                            text = "▶ " + stuff["MediaContainer"]["Video"][0]["@grandparentTitle"];
                            text_second = "S" + int.Parse(stuff["MediaContainer"][0]["Video"]["@parentIndex"].ToString()).ToString("00") + "E" + int.Parse(stuff["MediaContainer"]["Video"][0]["@index"].ToString()).ToString("00") + " " + stuff["MediaContainer"]["Video"][0]["@title"];
                        }

                    }
                    else
                    {
                        if (stuff["MediaContainer"]["Video"]["@type"] == "movie")
                        {
                            // Movie
                            text = "▶ " + stuff["MediaContainer"]["Video"]["@title"];
                            text_second = "";

                        }
                        else
                        {
                        
                        // TV Show
                        text = "▶ " + stuff["MediaContainer"]["Video"]["@grandparentTitle"];
                        text_second = "S"+ int.Parse(stuff["MediaContainer"]["Video"]["@parentIndex"].ToString()).ToString("00") + "E" + int.Parse(stuff["MediaContainer"]["Video"]["@index"].ToString()).ToString("00") + " "+ stuff["MediaContainer"]["Video"]["@title"];
                        }

                    }

                    label1.Text = text;
                    label5.Text = text_second;

                    UpdatePresence(text, text_second, new Assets()
                    {
                        LargeImageKey = "plex-app-icon", // change to icon in your app
                        LargeImageText = text + " - " + text_second,
                        SmallImageKey = "plex-app-icon" // change to icon in your app
                    });

                }
                else
                {
                    detected = false;
                    label1.Text = "Nothing Playing";
                    label5.Text = "";
                    client.ClearPresence();
                    now = DateTime.UtcNow;
                }

            return true;
        }

        private void UpdatePresence(string Details, string State,Assets PlexData)
        {
            client.SetPresence(new RichPresence()
            {
                Details = Details,
                State = State,
                Assets = PlexData,
                Timestamps = new Timestamps(now)
            });
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (!status)
            {
                timer1.Start();
                status = !status;
                button1.Text = "Stop";

            }
            else
            {
                timer1.Stop();
                detected = false;
                client.ClearPresence();
                status = !status;
                button1.Text = "Run";
                label1.Text = "Offline";
                label5.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
            string tick_s = tick.ToString();
            if ( (tick % 5) == 0)
            {
                GetPlexData();
            }
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/dhrinkino/PlexRichPresence");
            linkLabel1.LinkVisited = true;
        }
    }
}
