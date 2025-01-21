using System;
using System.Drawing;
using System.Drawing.Drawing2D; // Required for GraphicsPath
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
            var alertForm = new RoundedForm
            {
                Width = 350,
                Height = 250,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.WhiteSmoke, // Light background for the alert
                TopMost = true // Ensure the form is on top of all other windows
            };

            // Add a label with a message
            var messageLabel = new Label
            {
                Text = "PLEASE FILL DETAILS!!",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold), // Use a standard font
                ForeColor = Color.Black, // Black text for visibility
                Location = new Point(50, 70), // Adjusted for center alignment
                BackColor = Color.WhiteSmoke // Match the background for seamless look
            };
            alertForm.Controls.Add(messageLabel);

            // Add a button to close the alert
            var closeButton = new Button
            {
                Text = "Dismiss",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.Purple,
                ForeColor = Color.WhiteSmoke, // Contrast text color
                Location = new Point(130, 130), // Center the button
                AutoSize = true,
                FlatStyle = FlatStyle.Flat // Flat style for better appearance
            };
            closeButton.FlatAppearance.BorderSize = 0; // Remove button border
            closeButton.Click += (s, e) => alertForm.Close();
            alertForm.Controls.Add(closeButton);

            // Show the alert form as a dialog and activate it
            alertForm.Shown += (s, e) => alertForm.Activate(); // Ensure the form is activated
            alertForm.ShowDialog();
        }

        private static void OnExit(object sender, EventArgs e)
        {
            // Clean up resources
            _notifyIcon.Visible = false;
            Application.Exit();
        }
    }

    // Custom form class with rounded corners
    public class RoundedForm : Form
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // Radius for rounded corners
            path.AddArc(0, 0, radius, radius, 180, 90); // Top-left corner
            path.AddArc(Width - radius, 0, radius, radius, 270, 90); // Top-right corner
            path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90); // Bottom-right corner
            path.AddArc(0, Height - radius, radius, radius, 90, 90); // Bottom-left corner
            path.CloseFigure();
            this.Region = new Region(path); // Set the region of the form to the rounded rectangle
        }
    }
}
