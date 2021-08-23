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
        private bool first_run = false;
        public Form1()
        {
            InitializeComponent();
            label5.Text = null;
            notifyIcon1.Icon = SystemIcons.Application;
            notifyIcon1.BalloonTipTitle = "Rich Presence for Plex";
            notifyIcon1.BalloonTipText = "Rich presence is still running";
            string[] settings = null;
            try
            {
                settings = File.ReadAllLines("settings.dat");

                Properties.Settings.Default.DiscordID = settings[0];
                Properties.Settings.Default.PlexToken = settings[1];
                Properties.Settings.Default.AutoStart = settings[2];
                Properties.Settings.Default.PlexDirect = settings[3];
            }
            catch {
                MessageBox.Show("Please set a credentials before running");
            };
            if (Properties.Settings.Default.AutoStart == "True")
            {
                DiscordInitialize();
                first_run = !first_run;
                timer1.Start();
                status = !status;
                button1.Text = "Stop";
            }
        }


        public void DiscordInitialize()
        {
            client = new DiscordRpcClient(Properties.Settings.Default.DiscordID);
            client.Initialize();
        }

        private bool GetPlexData()
        {
            string response = null;
            try { 
                response = new WebClient().DownloadString("https://"+ Properties.Settings.Default.PlexDirect +":32400/status/sessions?X-Plex-Token=" + Properties.Settings.Default.PlexToken);
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
                        if (stuff["MediaContainer"]["Video"][0]["@type"] == "movie")
                        {
                            text = stuff["MediaContainer"]["Video"][0]["@title"];

                        }
                        else
                        {
                            text = stuff["MediaContainer"]["Video"][0]["@grandparentTitle"] + " " + stuff["MediaContainer"]["Video"][0]["@title"];
                        }

                    }
                    else
                    {
                        if (stuff["MediaContainer"]["Video"]["@type"] == "movie")
                        {
                            text = stuff["MediaContainer"]["Video"]["@title"];
                            text_second = "";

                        }
                        else
                        {
                            text = stuff["MediaContainer"]["Video"]["@grandparentTitle"];
                            text_second = stuff["MediaContainer"]["Video"]["@title"];
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
                    label1.Text = "Nothing Playing";
                    label5.Text = "";
                    client.ClearPresence();
                }

            return true;
        }

        private void UpdatePresence(string Details, string State,Assets PlexData)
        {
            client.SetPresence(new RichPresence()
            {
                Details = Details,
                State = State,
                Assets = PlexData
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
            if (first_run == false) {
                DiscordInitialize();
                first_run = !first_run;
            }
            

            if (!status)
            {
                timer1.Start();
                status = !status;
                button1.Text = "Stop";

            }
            else
            {
                timer1.Stop();
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
            Form2 f2 = new Form2();
            f2.ShowDialog();
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
