using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSReverse.TextureModder
{
    public class CharacterTexture
    {
        private JSR.Character Character;
        private String Name;
        private String FileName;
        private int NumPvrFiles;

        public CharacterTexture(JSR.Character character, String fileName, int numPvrFiles)
        {
            Character = character;
            Name = character.ToString();
            FileName = fileName;
            NumPvrFiles = numPvrFiles;
        }

        public String GetName()
        {
            return Name;
        }

        public String GetFileName()
        {
            return FileName;
        }

        public int GetNumPvrFiles()
        {
            return NumPvrFiles;
        }

        public JSR.Character GetCharacter()
        {
            return Character;
        }
    }
}
