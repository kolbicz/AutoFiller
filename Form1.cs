using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using Microsoft.Win32;

namespace AutoFiller
{
    public partial class Form1 : Form
    {
        private static String TextToSend;
        private static String WindowTitle;
        private static Int32 TimeToWait;

        private static System.Timers.Timer aTimer;

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static String RegPath = @"HKEY_CURRENT_USER\Software\AutoFiller";

        public Form1()
        {
            InitializeComponent();

            try
            {
                TextToSend = (string)Registry.GetValue(RegPath, "TextToSend", null);
            }
            catch
            {
                MessageBox.Show("Not able to read TextToSend from the registry (REG_SZ)!", "Error");
                System.Environment.Exit(-1);
            }

            try
            {
                WindowTitle = (string)Registry.GetValue(RegPath, "WindowTitle", null);
            }
            catch
            {
                MessageBox.Show("Not able to read WindowTitle from the registry (REG_SZ)!", "Error");
                System.Environment.Exit(-1);
            }

            try
            {
                TimeToWait = (Int32)Registry.GetValue(RegPath, "TimeToWait", null);
            }
            catch
            {
                MessageBox.Show("Not able to read TimeToWait from the registry (REG_DWORD)!", "Error");
                System.Environment.Exit(-1);
            }

            SetTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private static void CheckWindow()
        {

            IntPtr wdwIntPtr = FindWindow(null, WindowTitle);

            if (wdwIntPtr != IntPtr.Zero)
            {
                aTimer.Stop(); // stop the timer as soon the window was found
                SetForegroundWindow(wdwIntPtr); // bring the window to the foreground
                System.Threading.Thread.Sleep(TimeToWait); // wait for time defined in the registry
                SendKeys.SendWait(TextToSend); // send the text defined in the registry
                SendKeys.SendWait("{ENTER}"); // send submit
                SendKeys.Flush(); // wait for application to respond
                System.Threading.Thread.Sleep(10000); // wait 10 seconds - just to make sure the windows is gone
                aTimer.Start(); // restart the timer in case the window will show again
                return;
            }
        }

        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(1000); // check for window every second
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            CheckWindow();
        }
    }
}
