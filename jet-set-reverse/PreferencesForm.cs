using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JSReverse
{
    public partial class PreferencesForm : JSRForm
    {
        public PreferencesForm()
        {
            InitializeComponent();
            base.InitializeComponent();

            defaultOutputDirectoryTextBox.Text = Preferences.DEFAULT_OUTPUT_DIRECTORY;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Preferences.Save(defaultOutputDirectoryTextBox.Text);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            defaultOutputDirectoryTextBox.Text = String.Empty;
        }
    }
}
