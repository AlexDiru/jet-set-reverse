using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSReverse.TextureModder;
using System.Drawing.Imaging;

namespace JSReverse
{
    public partial class TextureModderForm : JSRForm
    {

        private enum TextureModderMode
        {
            Character, Stage
        }

        private TextureModderMode Mode = TextureModderMode.Character;
        private Textures textures = new Textures();
        private ImportFileType importFileType = ImportFileType.PVR;

        public TextureModderForm()
        {
            InitializeComponent();
            base.InitializeComponent();
            PopulateDropDownList();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            ExportedTexturesDirectoryTextBox.Text = Preferences.DEFAULT_OUTPUT_DIRECTORY;
            FilesToImportDirectory.Text = Preferences.DEFAULT_OUTPUT_DIRECTORY;
            ExportedBinOutputDirectoryTextBox.Text = Preferences.DEFAULT_OUTPUT_DIRECTORY;
        }

        private void PopulateDropDownList()
        {
            comboBox1.Items.Clear();
            if (Mode == TextureModderMode.Character)
                comboBox1.Items.AddRange(textures.characterTextures.Select(ct => ct.GetName()).ToArray());
            else if (Mode == TextureModderMode.Stage)
                comboBox1.Items.AddRange(textures.StageTextures.Select(st => st.GetName()).ToArray());

            comboBox1.SelectedIndex = 0;
            selectItemLabel.Text = "Select " + Mode.ToString() + ":";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == TextureModderMode.Character)
            {
                var sc = textures.characterTextures.Where(ct => ct.GetName() == comboBox1.Text).First();
                if (sc != null)
                {
                    binFileLocationLabel.Text = "Location of " + sc.GetFileName() + ":";
                }
            }
            else if (Mode == TextureModderMode.Stage)
            {
                var ss = textures.StageTextures.Where(st => st.GetName() == comboBox1.Text).First();
                if (ss != null)
                {
                    binFileLocationLabel.Text = "Location of " + ss.GetFileName() + ":";
                }
            }
        }

        private void binFileChooser_Click(object sender, EventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                BinToExportFilePathTextBox.Text = fileDialog.FileName;
        }

        private void outDirFileChooseBtn_Click(object sender, EventArgs e)
        {
            var fileDialog = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                ExportedTexturesDirectoryTextBox.Text = fileDialog.SelectedPath;
        }

        private void ValidateControlsForExport()
        {
            ExportErrorLabel.Text = String.Empty;
            ExportSuccessLabel.Text = String.Empty;

            if (String.IsNullOrEmpty(BinToExportFilePathTextBox.Text))
            {
                ExportErrorLabel.Text = "Please locate the .BIN file";
                return;
            }

            if (String.IsNullOrEmpty(ExportedTexturesDirectoryTextBox.Text))
            {
                ExportErrorLabel.Text = "Please choose an output directory";
                return;
            }
        }

        private void convertToPVRButton_Click(object sender, EventArgs e)
        {
            ValidateControlsForExport();

            try
            {
                byte[] binData = System.IO.File.ReadAllBytes(BinToExportFilePathTextBox.Text);
                var pvrData = JSReverse.JSR.BinToPVR(binData);
                JSReverse.JSR.SavePVRFiles(pvrData, ExportedTexturesDirectoryTextBox.Text);
            }
            catch (Exception ex)
            {
                ExportErrorLabel.Text = ex.Message;
            }

            ExportSuccessLabel.Text = "Success";
        }

        private void pvrFileDirButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                FilesToImportDirectory.Text = fileDialog.SelectedPath;
        }

        private void binDirBrowseButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                ExportedBinOutputDirectoryTextBox.Text = fileDialog.SelectedPath;
        }

        private void convertToBinFileButton_Click(object sender, EventArgs e)
        {
            //Are we using PVR or BMP?
            importFileType = bmpRadioButton.Checked ? ImportFileType.PNG : ImportFileType.PVR;

            ImportErrorLabel.Text = String.Empty;
            ImportSuccessLabel.Text = String.Empty;

            if (String.IsNullOrEmpty(ExportedBinOutputDirectoryTextBox.Text))
            {
                ImportErrorLabel.Text = "Please enter an output directory for the .bin file";
                return;
            }

            if (String.IsNullOrEmpty(FilesToImportDirectory.Text))
            {
                ImportErrorLabel.Text = "Please enter a directory for where the ." + importFileType.ToString() + " file(s) are located";
                return;
            }

            //If using bitmaps, make sure a config file exists
            PVRConfig config = null;
            if (importFileType == ImportFileType.PNG)
            {
                try
                {
                    config = new PVRConfig();
                    config.Load(FilesToImportDirectory.Text + "/" + Preferences.PVR_CONFIG_FILENAME);
                }
                catch
                {
                    ImportErrorLabel.Text = "Cannot find the file \""+ Preferences.PVR_CONFIG_FILENAME + "\". Please locate it";
                    return;
                }
            }

            int numFiles = 0;
            String outFile = null;
            JSR.Character character = JSR.Character.Gum;
            JSR.Stage stage = JSR.Stage.ShibuyaArea3Part1;

            if (Mode == TextureModderMode.Character)
            {
                CharacterTexture sc;
                try
                {
                    sc = textures.characterTextures.Where(ct => ct.GetName() == comboBox1.Text).First();
                }
                catch (Exception ex)
                {
                    ImportErrorLabel.Text = "Please select a character";
                    return;
                }
                numFiles = sc.GetNumPvrFiles();
                outFile = sc.GetFileName();
                character = sc.GetCharacter();
            }
            else if (Mode == TextureModderMode.Stage)
            {
                StageTexture ss;
                try
                {
                    ss = textures.StageTextures.Where(st => st.GetName() == comboBox1.Text).First();
                }
                catch (Exception ex)
                {
                    ImportErrorLabel.Text = "Please select a stage";
                    return;
                }
                numFiles = ss.GetNumPvrFiles();
                outFile = ss.GetFileName();
                stage = ss.GetStage();
            }

            //Find the RIPPED_X files
            List<List<Byte>> pvrData = new List<List<Byte>>();
            try
            {
                for (int i = 0; i < numFiles; i++)
                {
                    if (importFileType == ImportFileType.PVR)
                    {
                        pvrData.Add(new List<Byte>());
                        pvrData[i].AddRange(System.IO.File.ReadAllBytes(FilesToImportDirectory.Text + "/RIPPED_" + i + ".pvr"));
                        Console.WriteLine("Added RIPPED_" + i + ".pvr");
                    }
                    else if (importFileType == ImportFileType.PNG)
                    {
                        //Load the bitmap - if the png file doesn't exist, the decode may have failed so we need to hunt for the PVR File
                        Bitmap bitmap;
                        try
                        {
                            bitmap = (Bitmap)Bitmap.FromFile(FilesToImportDirectory.Text + "/RIPPED_" + i + ".png");
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            //Look for PVR
                            pvrData.Add(new List<Byte>());
                            pvrData[i].AddRange(System.IO.File.ReadAllBytes(FilesToImportDirectory.Text + "/RIPPED_" + i + ".pvr"));
                            Console.WriteLine("Added RIPPED_" + i + ".pvr");
                            continue;
                        }
                        var data = config.Get(i);

                        //Bit of a hack, but run pvrconv from command line, saves PVR importing having to be done
                        //First convert png to bmp
                        var adjustedDepthBitmap = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        using (Graphics g = Graphics.FromImage(adjustedDepthBitmap))
                        {
                            g.DrawImage(bitmap, new Rectangle(0, 0, adjustedDepthBitmap.Width, adjustedDepthBitmap.Height));
                            adjustedDepthBitmap.Save(FilesToImportDirectory.Text + "/RIPPED_" + i + ".bmp", ImageFormat.Bmp);
                        }

                        //Get the location of pvrconv
                        String exeLoc = "\"" + (Preferences.PVR_CONV_EXE_LOCATION == String.Empty ? String.Empty : Preferences.PVR_CONV_EXE_LOCATION + "/");
                        exeLoc += "pvrconv.exe\"";

                        var cmd = data.ToPVRConvFlags() + " \"" + FilesToImportDirectory.Text + "/RIPPED_" + i + ".bmp\"";

                        var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = exeLoc;
                        process.StartInfo.Arguments = cmd;
                        process.Start();
                        process.WaitForExit();
                        Console.WriteLine("Added RIPPED_" + i + ".png");
                    
                        //Add PVR file data
                        pvrData.Add(new List<Byte>());
                        pvrData[i].AddRange(System.IO.File.ReadAllBytes(FilesToImportDirectory.Text + "/RIPPED_" + i + ".pvr"));
                        Console.WriteLine("Added RIPPED_" + i + ".pvr");

                        //Clean up files
                        System.IO.File.Delete(FilesToImportDirectory.Text + "/RIPPED_" + i + ".bmp");
                        System.IO.File.Delete(FilesToImportDirectory.Text + "/RIPPED_" + i + ".pvr");
                    }
                }
            }
            catch (Exception ex)
            {
                ImportErrorLabel.Text = ex.Message;
                return;
            }

            try
            {
                if (Mode == TextureModderMode.Character)
                    System.IO.File.WriteAllBytes(ExportedBinOutputDirectoryTextBox.Text + "/" + outFile, JSReverse.JSR.PVRToBin(character, pvrData));
                else if (Mode == TextureModderMode.Stage)
                    System.IO.File.WriteAllBytes(ExportedBinOutputDirectoryTextBox.Text + "/" + outFile, JSReverse.JSR.PVRToTXP(stage, pvrData));
            }
            catch (Exception ex)
            {
                ImportErrorLabel.Text = ex.Message;
                return;
            }

            ImportSuccessLabel.Text = "Success!";

        }

        private void characterRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Mode = TextureModderMode.Character;
            PopulateDropDownList();
        }

        private void stageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Mode = TextureModderMode.Stage;
            PopulateDropDownList();
        }

        private void convertToPNGButton_Click(object sender, EventArgs e)
        {
            ValidateControlsForExport();

            try
            {
                byte[] binData = System.IO.File.ReadAllBytes(BinToExportFilePathTextBox.Text);
                var pvrData = JSReverse.JSR.BinToPVR(binData);

                var config = new TextureModder.PVRConfig();
                JSReverse.JSR.SavePVRAsPNGFiles(config, pvrData, ExportedTexturesDirectoryTextBox.Text);
                config.Save(ExportedTexturesDirectoryTextBox.Text + "/" + Preferences.PVR_CONFIG_FILENAME);
            }
            catch (Exception ex)
            {
                ExportErrorLabel.Text = ex.Message;
                return;
            }

            ExportSuccessLabel.Text = "Success";
        }

        private enum ImportFileType
        {
            PNG,
            PVR
        }

        private void viewTexturesButton_Click(object sender, EventArgs e)
        {
            ValidateControlsForExport();
        }

    }
}
