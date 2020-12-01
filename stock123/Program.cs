using System;
using System.Collections.Generic;
using System.Windows.Forms;
using stock123.app;
using xlib.framework;

namespace stock123
{
    static class Program
    {
        static bool runWithoutInstall = true;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            int h = Screen.PrimaryScreen.WorkingArea.Height;
            bool miniMode = false;
            if (args.Length > 0)
            {
                miniMode = true;
                return;
            }

            string path = System.IO.Directory.GetCurrentDirectory();// Application.ExecutablePath;
            //System.IO.Directory.SetCurrentDirectory(Application.UserAppDataPath);
            if (!runWithoutInstall)
            {
                path = Application.UserAppDataPath;
            }
            //path

            xFileManager.RootDir = Application.StartupPath;
            xFileManager.UserDir = path + "\\vnChart\\";// Application.StartupPath;
            //xFileManager.UserDir = "C:" + "\\vnChart\\";// Application.StartupPath;
            try
            {
                System.IO.Directory.CreateDirectory(xFileManager.UserDir);
                System.IO.Directory.CreateDirectory(xFileManager.UserDir + "\\data");
                //System.IO.Directory.CreateDirectory("vnChart\\data");
            }
            catch (Exception e)
            {
            }

            //MessageBox.Show("C:\\");

            if (xFileManager.RootDir[xFileManager.RootDir.Length - 1] != '\\')
            {
                xFileManager.RootDir = xFileManager.RootDir + "\\";
            }
            //MessageBox.Show("1");
            MainApplication.mMiniMode = miniMode;
            ////    read windows state
            //MessageBox.Show("11");
            xDataInput di = xFileManager.readFile("windstate", false);
            //MessageBox.Show("12");
            int x = 0;
            int y = 0;
            w = w / 2;
            h = h / 2;
            FormWindowState state = FormWindowState.Maximized;
            //MessageBox.Show("2");
            if (w < 600) w = 600;
            if (h < 400) h = 400;

            if (di != null)
            {
                state = (FormWindowState)di.readInt();
                x = di.readInt();
                y = di.readInt();
                w = di.readInt();
                h = di.readInt();
            }
            //MessageBox.Show("3");

            MainApplication app = new MainApplication(w, h);

            app.SetBounds(x, y, w, h, BoundsSpecified.All);
            //app.DesktopLocation = new System.Drawing.Point(x, y);
            app.DesktopLocation = new System.Drawing.Point(x, y);

            app.WindowState = state;
            app.StartPosition = FormStartPosition.Manual;
            //MessageBox.Show("4");
            Application.Run(app);
            //MessageBox.Show("1000");
            if (app.mShouldSaveWindowState)
            {
                xDataOutput o = new xDataOutput(100);
                if (app.WindowState == FormWindowState.Normal)
                {
                    o.writeInt((int)app.WindowState);
                    o.writeInt(app.DesktopLocation.X);
                    o.writeInt(app.DesktopLocation.Y);
                    o.writeInt(app.Size.Width);
                    o.writeInt(app.Size.Height);
                }
                else
                {
                    o.writeInt((int)app.WindowState);
                    o.writeInt(x);
                    o.writeInt(y);
                    o.writeInt(w);
                    o.writeInt(h);
                }
                xFileManager.saveFile(o, "windstate");
            }
        }
    }
}