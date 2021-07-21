using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secondbot_windows_tray
{
    public partial class Secondbot : Form
    {
        public Secondbot()
        {
            InitializeComponent();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Update();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Secondbot_Load(object sender, EventArgs e)
        {
            var th = new Thread(BotThread);
            th.Start();

        }
        protected bool exited = false;
        protected string output = "Starting please wait";
        protected string lastline = "";
        protected List<string> lines = new List<string>();
        protected void BotThread()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "BetterSecondbot.exe",
                    Arguments = "",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                if(line != lastline)
                {
                    lastline = line;
                    lock (lines)
                    {
                        lines.Add(line);
                        if(lines.Count > 35)
                        {
                            lines.RemoveRange(0, 10);
                        }
                    }
                }
            }
            process.WaitForExit();
            exited = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                string output = "";
                List<string> cmdout = lines;
                foreach (string a in cmdout)
                {
                    output += a;
                    output += System.Environment.NewLine;
                }
                if (output != textBox1.Text)
                {
                    textBox1.Text = output;
                }
            }
            if(exited == false)
            {
                timer1.Enabled = false;
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Update();
        }
    }
}
