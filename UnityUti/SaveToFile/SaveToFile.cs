using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PlugRMK.GenericUti
{
    public static class SaveToFile
    {
        const string FILE_NAME = "mixed_1";
        const string EXTENSION_NAME = ".gamedata";
        static readonly string PATH = Path.Combine(Application.persistentDataPath, FILE_NAME + EXTENSION_NAME);

        public static void Save(string content)
        {
            File.WriteAllText(PATH, content);
        }

        public static bool TryLoad(out string content)
        {
            if (File.Exists(PATH))
            {
                content = File.ReadAllText(PATH);
                return true;
            }

            content = "";
            return false;
        }
    }
}
