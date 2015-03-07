using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSReverse
{
    class Program
    {
        [STAThread]
        public static void Main(String[] args)
        {
            Preferences.Load();
            System.Windows.Forms.Application.Run(new InitialForm());
        }
    }
}
