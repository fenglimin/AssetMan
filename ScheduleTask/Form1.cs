using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ScheduleTask
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer workTimer = new System.Timers.Timer(72000000);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoCheck(@"GetNetWorth.bat", "");
            workTimer.Elapsed += OnTimedEvent;
            workTimer.AutoReset = true;
            workTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            DoCheck(@"GetNetWorth.bat", "");
        }

        private void DoCheck(string exeFile, string workingDir)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo(exeFile) { UseShellExecute = false };
                if (workingDir != "")
                {
                    processStartInfo.WorkingDirectory = workingDir;
                }

                var cmd = Process.Start(processStartInfo);
                cmd?.WaitForExit();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
