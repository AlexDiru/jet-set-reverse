using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSReverse.TextureModder
{
    public class StageTexture
    {
        private JSR.Stage Stage;
        private String FileName;
        private int NumPvrFiles;

        public StageTexture(JSR.Stage stage, String fileName, int numPvrFiles)
        {
            Stage = stage;
            FileName = fileName;
            NumPvrFiles = numPvrFiles;
        }

        public String GetName()
        {
            return Stage.ToString();
        }

        public JSR.Stage GetStage()
        {
            return Stage;
        }

        public int GetNumPvrFiles()
        {
            return NumPvrFiles;
        }

        public String GetFileName()
        {
            return FileName;
        }
    }
}
