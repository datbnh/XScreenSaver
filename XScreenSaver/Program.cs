using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var firstArgument = args[0].ToLower().Trim().Substring(0, 2);

                if (firstArgument == "/s") // show screen saver
                {
                    LaunchScreenSaverForm();
                }
                else if (firstArgument == "/p") // preview screen saver
                {
                    LaunchScreenSaverForm();
                }
                else if (firstArgument == "/c") // configure screen saver
                {

                }
            }
            else // no argument passed, show screen saver
            {
                LaunchScreenSaverForm();
            }
        }

        static void LaunchScreenSaverForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            //loops through all the computer's screens (monitors)
            foreach (Screen screen in Screen.AllScreens)
            {
                Form1 screensaver = new Form1(screen);
                screensaver.Show();
                Application.Run(screensaver);
                //Settings settings = new Settings();
                //settings.Show();
                //Application.Run(settings);

            }
        }

    }
}
