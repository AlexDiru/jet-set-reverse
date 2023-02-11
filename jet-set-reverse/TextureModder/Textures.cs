using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSReverse.TextureModder
{
    public class Textures
    {
        public List<CharacterTexture> characterTextures = new List<CharacterTexture>();
        public List<StageTexture> StageTextures = new List<StageTexture>();

        public Textures()
        {
            characterTextures.Add(new CharacterTexture(JSR.Character.Gum, "GUMTXR.BIN", 5));
            characterTextures.Add(new CharacterTexture(JSR.Character.Beat, "EREKITXR.BIN", 5));
            characterTextures.Add(new CharacterTexture(JSR.Character.Mew, "BISTXR.BIN", 4));
            characterTextures.Add(new CharacterTexture(JSR.Character.Yoyo, "YOYOTXR.BIN", 4));
            characterTextures.Add(new CharacterTexture(JSR.Character.Slate, "CODETXR.BIN", 5));
            characterTextures.Add(new CharacterTexture(JSR.Character.Combo, "CONPOTXR.BIN", 5));
            characterTextures.Add(new CharacterTexture(JSR.Character.NoiseTank, "RIVAL_NTANKTXR.BIN", 6));
            characterTextures.Add(new CharacterTexture(JSR.Character.Tab, "CORNTXR.BIN", 5));
            //characterTextures.Add(new CharacterTexture(JSR.Character.GojiDOESNTWORK, "GOJITXR.BIN", 4));
            characterTextures.Add(new CharacterTexture(JSR.Character.Piranha, "NATTXR.BIN", 5));
            characterTextures.Add(new CharacterTexture(JSR.Character.Cube, "RECOTXR.BIN", 5));
            characterTextures.Add(new CharacterTexture(JSR.Character.Garam, "PLUGTXR.BIN", 4));
            characterTextures.Add(new CharacterTexture(JSR.Character.Pots, "POTSTXR.BIN", 3));

            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaAreaAll, "TEXPACK_ST1_ALL_00.TXP", 192));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea2Part1, "TEXPACK_ST1_AREA2_00.TXP", 18));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea2Part2, "TEXPACK_ST1_AREA2_01.TXP", 14));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea3Part1, "TEXPACK_ST1_AREA3_00.TXP", 27));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea3Part2, "TEXPACK_ST1_AREA3_01.TXP", 21));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea4Part1, "TEXPACK_ST1_AREA4_00.TXP", 19));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea4Part2, "TEXPACK_ST1_AREA4_01.TXP", 13));
            StageTextures.Add(new StageTexture(JSR.Stage.ShibuyaArea4Part3, "TEXPACK_ST1_AREA4_02.TXP", 7));
        }
    }
}
