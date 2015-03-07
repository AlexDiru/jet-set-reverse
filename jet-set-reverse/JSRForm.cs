using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JSReverse
{
    public class JSRForm : System.Windows.Forms.Form
    {
        private static readonly String VERSION_NUMBER = "0.1";
        private static readonly String CREDITS = "Created by Alex Spedding @AlexDiru";

        private static String CurrentVersion = "v" + VERSION_NUMBER + "." + DateTime.Now.ToString("yyMMdd");
        public static String GetWindowTitle() { return "Jet Set Reverse " + CurrentVersion + " " + CREDITS; }

        public void InitializeComponent()
        {
            Text = GetWindowTitle();
            Icon = Properties.Resources.Icon;
        }
    }
}
