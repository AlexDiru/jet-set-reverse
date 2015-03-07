using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace JSReverse
{
    public partial class InitialForm : JSRForm
    {
        public InitialForm()
        {
            InitializeComponent();
            base.InitializeComponent();
        }

        private void characterModderButton_Click(object sender, EventArgs e)
        {
            (new TextureModderForm()).Show();
        }

        private void preferencesButton_Click(object sender, EventArgs e)
        {
            (new PreferencesForm()).Show();
        }
    }
}
