using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSReverse
{
    public class JSR
    {
        public class CannotDecodePvrException : Exception
        {
            public CannotDecodePvrException() : base() 
            {
            }
        }

        private static String ToString(byte[] byteArr)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < byteArr.Length; i++)
                sb.Append((char)(int)byteArr[i]);
            return sb.ToString();
        }

        private static List<List<Byte>> To2DByteArray(String[] data)
        {
            var outData = new List<List<Byte>>();
            foreach (String datum in data)
            {
                outData.Add(new List<Byte>());
                foreach (char by in datum)
                    outData[outData.Count - 1].Add((byte)(int)by);
            }
            return outData;
        }

        private static void SavePVRFile(List<Byte> pvrData, String outputDirectory, int index) 
        {
            System.IO.File.WriteAllBytes(outputDirectory + "/RIPPED_" + index + ".pvr", pvrData.ToArray());
        }

        private static void SavePVRAsPNGFile(TextureModder.PVRConfig config, List<Byte> pvrData, String outputDirectory, int index)         
        {
            var texture = new VrSharp.PvrTexture.PvrTexture(pvrData.ToArray());
            config.Add(texture);
            var bitmap = texture.ToBitmap();
            bitmap.Save(outputDirectory + "/RIPPED_" + index + ".png");
            bitmap.Dispose();
        }
        

        public static void SavePVRFiles(List<List<Byte>> pvrData, String outputDirectory)
        {
            int index = 0;
            foreach (var pvr in pvrData)
                SavePVRFile(pvr, outputDirectory, index++);
        }


        public static void SavePVRAsPNGFiles(TextureModder.PVRConfig config, List<List<Byte>> pvrData, String outputDirectory)
        {
            int index = 0;
            foreach (var pvr in pvrData)
            {
                try
                {
                    SavePVRAsPNGFile(config, pvr, outputDirectory, index);
                }
                catch (CannotDecodePvrException)
                {
                    SavePVRFile(pvr, outputDirectory, index);
                }
                index++;
            }
        }

        /// <summary>
        /// Converts a .bin to one or more .pvr files
        /// Each .pvr can be split by the occurence of GBIX
        /// </summary>
        /// <param name="binData">The data in the .bin file</param>
        /// <returns>An array of .pvr file data</returns>
        public static List<List<Byte>> BinToPVR(byte[] binData)
        {
            int currentPVR = -1;
            String gbixChecker = String.Empty;
            var outData = new List<List<Byte>>();

            for (int i = 0; i < binData.Length; i++)
            {
                bool ignoreData = false;
                Byte data = binData[i];
                if (data == 71)
                    gbixChecker = "G";
                else if (data == 66 && gbixChecker == "G")
                    gbixChecker = "GB";
                else if (data == 73 && gbixChecker == "GB")
                    gbixChecker = "GBI";
                else if (data == 88 && gbixChecker == "GBI")
                {
                    if (currentPVR != -1)
                        outData[currentPVR].RemoveRange(outData[currentPVR].Count() - 3, 3); //Remove GBI
                    currentPVR++;
                    outData.Add(new List<Byte>());
                    outData[currentPVR].Add((byte)(int)'G');
                    outData[currentPVR].Add((byte)(int)'B');
                    outData[currentPVR].Add((byte)(int)'I');
                    gbixChecker = String.Empty;
                }
                else
                    gbixChecker = String.Empty;

                if (!ignoreData && currentPVR != -1)
                    outData[currentPVR].Add(data);

            }

            return outData;
        }

        public static byte[] PVRToTXP(Stage stage, byte[][] pvrData)
        {
            List<Byte> outData = new List<Byte>();
            //writePTEXHeader(character, outData);
            foreach (byte[] pvr in pvrData)
            {
                foreach (byte data in pvr)
                    outData.Add(data);
            }

            return outData.ToArray();
        }


        public static byte[] PVRToTXP(Stage stage, List<List<Byte>> pvrData)
        {
            return PVRToTXP(stage, ByteList2DToArray2D(pvrData));
        }


        /// <summary>
        /// Merges one or .pvr files into a .bin file
        /// </summary>
        /// <param name="pvrData">The array of data in the .pvr files</param>
        /// <returns>The data of the .bin file</returns>
        public static byte[] PVRToBin(Character character, byte[][] pvrData)
        {
            List<Byte> outData = new List<Byte>();
            writePTEXHeader(character, outData);
            foreach (byte[] pvr in pvrData)
            {
                foreach (byte data in pvr)
                    outData.Add(data);
            }

            return outData.ToArray();
        }

        private static byte[][] ByteList2DToArray2D(List<List<Byte>> pvrData) 
        {
            var pvrArr = new byte[pvrData.Count()][];
            for (int i = 0; i < pvrData.Count(); i++)
                pvrArr[i] = pvrData[i].ToArray();
            return pvrArr;
        }

        public static byte[] PVRToBin(Character character, List<List<Byte>> pvrData)
        {
            return PVRToBin(character, ByteList2DToArray2D(pvrData));
        }

        private static void writePTEXHeader(Character character, List<Byte> pvrFile)
        {
            if (character == Character.Gum)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_GUM.getData());
            else if (character == Character.Beat)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_BEAT.getData());
            else if (character == Character.Mew)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_MEW.getData());
            else if (character == Character.Yoyo)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_YOYO.getData());
            else if (character == Character.Slate)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_SLATE.getData());
            else if (character == Character.Combo)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_COMBO.getData());
            else if (character == Character.NoiseTank)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_NOISETANK.getData());
            else if (character == Character.Tab)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_TAB.getData());
            else if (character == Character.GojiDOESNTWORK)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_GOJI.getData());
            else if (character == Character.Piranha)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_PIRANHA.getData());
            else if (character == Character.Garam)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_GARAM.getData());
            else if (character == Character.Pots)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_POTS.getData());
            else if (character == Character.Cube)
                pvrFile.InsertRange(0, PTEX.PTEXHEADER_CUBE.getData());
            else
                throw new Exception("NO HEADER TO WRITE");
        }

        public enum Character
        {
            Gum,
            Beat,
            Mew,
            Yoyo,
            Slate,
            Combo,
            NoiseTank,
            Tab,
            Piranha,
            Garam,
            Pots,
            Cube,
            GojiDOESNTWORK
        }

        public enum Stage
        {
            ShibuyaAreaAll,
            ShibuyaArea2Part1,
            ShibuyaArea2Part2,
            ShibuyaArea3Part1,
            ShibuyaArea3Part2,
            ShibuyaArea4Part1,
            ShibuyaArea4Part2,
            ShibuyaArea4Part3
        }
    }
}
