using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Tower
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {   //auto-matically made
            //Make the visual style
            Application.EnableVisualStyles();
            //Sets the application-wide default for bleow line property defined on certain controls.
            Application.SetCompatibleTextRenderingDefault(false);
            //Begins running a standard application message loop on the current thread.
            Application.Run(new Form1());
        }
    }
}
