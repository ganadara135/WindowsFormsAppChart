using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppChart
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
            Application.Run(new Form1());
            // Show the chart
            //var form = new Form();
            //form.Text = "Pump Curve Chart";
            //form.Controls.Add(chart);
            //form.Width = 800;
            //form.Height = 600;
            //Application.Run(form);
        }
    }
}
