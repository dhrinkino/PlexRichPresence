using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Plex_Discord_Presence
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.DiscordID;
            textBox2.Text = Properties.Settings.Default.PlexToken;
            textBox3.Text = Properties.Settings.Default.PlexDirect;
            if (Properties.Settings.Default.AutoStart == "True")
            {
                checkBox1.Checked = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            // SAVE button
            Properties.Settings.Default.DiscordID = textBox1.Text;
            Properties.Settings.Default.PlexToken = textBox2.Text;
            Properties.Settings.Default.PlexDirect = textBox3.Text;

            string[] settings =
           {
                    Properties.Settings.Default.DiscordID, Properties.Settings.Default.PlexToken, checkBox1.Checked.ToString(), Properties.Settings.Default.PlexDirect
                };
            File.WriteAllLines("settings.dat", settings);
            MessageBox.Show("New settings was applied and saved to file settings.dat");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] settings = null;
            try
            {
                settings = File.ReadAllLines("settings.dat");
            }
            catch
            {
                MessageBox.Show("settings.dat not found");
            }
            finally {
                textBox1.Text = settings[0];
                textBox2.Text = settings[1];
                textBox3.Text = settings[3];
                Properties.Settings.Default.DiscordID = textBox1.Text;
                Properties.Settings.Default.PlexToken = textBox2.Text;
                Properties.Settings.Default.PlexDirect = textBox3.Text;
                MessageBox.Show("New credentials was imported from settings.dat");
            };

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
