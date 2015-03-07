using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JSReverse
{
    class Preferences
    {
        public static String FILE = "prefs.dat";
        public static String DEFAULT_OUTPUT_DIRECTORY = String.Empty;
        public static String PVR_CONFIG_FILENAME = "pvrconf.jsr";
        public static String PVR_CONV_EXE_LOCATION = "C:/Users/Alex/Dropbox/Jet Set Reverse/jet-set-reverse";

        public static void Save(String defaultOutputDirectory)
        {
            DEFAULT_OUTPUT_DIRECTORY = defaultOutputDirectory;

            Save();
        }

        private static void Save()
        {
            List<String> contents = new List<String>();
            contents.Add(DEFAULT_OUTPUT_DIRECTORY);
            File.WriteAllLines(FILE, contents);
        }

        public static void Load()
        {
            try
            {
                String[] contents = File.ReadAllLines(FILE);

                DEFAULT_OUTPUT_DIRECTORY = contents[0];
            }
            catch (IOException)
            {
                Save();
            }
            catch (Exception)
            {
                Console.WriteLine("Corrupted Preference File");
            }
        }
    }
}
