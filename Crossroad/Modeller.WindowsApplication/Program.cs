using System;
using System.Windows.Forms;

namespace Modeller.WindowsApplication
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            /*string ffor = String.Empty;
            for (int i = 0; i < 48; i++)
            {
                //ffor += string.Format("for (w_{0} = from; w_{1} <= to; w_{2} += step) ", i, i, i);
                ffor += string.Format("prinf(\"w_{0} = %lf\",w_{1})", i, i);
            }*/

            var a = Math.Atan(4.24);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModellerWindow());
        }
    }
}