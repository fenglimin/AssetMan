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
        private System.Timers.Timer workTimer = new System.Timers.Timer(2*60*60*1000);
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
            var week = DateTime.Now.DayOfWeek;
            if (week == DayOfWeek.Saturday || week == DayOfWeek.Sunday) return;

            var hour = DateTime.Now.Hour;
            if (hour < 9 || hour > 22) return;

            DoCheck(@"GetNetWorth.bat", "");
        }

        private void DoCheck(string exeFile, string workingDir)
        {
            try
            {
                label1.Text = "最后一次运行 " + DateTime.Now;
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
