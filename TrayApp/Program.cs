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
                ContextMenuStrip = new ContextMenuStrip
                {
                    Items = { new ToolStripMenuItem("Exit", null, OnExit) }
                },
                Text = "Tray Alert App"
            };

            // Start the alert loop
            StartAlertLoop();

            // Run the application
            Application.Run();
        }

        private static void StartAlertLoop()
        {
            // Create a background thread for alerting
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
                ShowStyledAlert();
            }
        }

        private static void ShowStyledAlert()
        {
            // Create and show a custom alert form
            var alertForm = new Form
            {
                Width = 400,
                Height = 200,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.LightBlue,
                TopMost = true
            };

            // Add a label with a message
            var messageLabel = new Label
            {
                Text = "This is your styled alert message!",
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(50, 50)
            };
            alertForm.Controls.Add(messageLabel);

            // Add a button to close the alert
            var closeButton = new Button
            {
                Text = "Dismiss",
                Font = new Font("Arial", 12, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Location = new Point(150, 120),
                AutoSize = true
            };
            closeButton.Click += (s, e) => alertForm.Close();
            alertForm.Controls.Add(closeButton);

            // Show the alert form as a dialog
            alertForm.ShowDialog();
        }

        private static void OnExit(object sender, EventArgs e)
        {
            // Clean up resources
            _notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
