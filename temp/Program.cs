using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IDeviceCommunication device;
            if (commandLine == "--test")
            {
                device = new FakeCommunitaction();
            }
            else
            {
                device = new DeviceCommunication();
            }

            Application.Run(new Form1(device));
        }
    }
}
