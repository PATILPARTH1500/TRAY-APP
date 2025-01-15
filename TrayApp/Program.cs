using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TrayApp
{
    static class Program
    {
        private static NotifyIcon _notifyIcon;
        private static Thread _alertThread;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize NotifyIcon
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };
            
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
            _notifyIcon.Text = "Tray Alert App";

            // Start the alert loop
            StartAlertLoop();

            // Run the application
            Application.Run();
        }

        private static void StartAlertLoop()
        {
            _alertThread = new Thread(AlertLoop)
            {
                IsBackground = true
            };
            _alertThread.Start();
        }

        private static void AlertLoop()
        {
            while (true)
            {
                Thread.Sleep(300000); // Wait for 5 minutes
                ShowAlertMessage();
            }
        }

        private static void ShowAlertMessage()
        {
            // Show a simple message box as the alert
            MessageBox.Show("This is your alert message!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void OnExit(object sender, EventArgs e)
        {
            // Clean up resources
            _notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
