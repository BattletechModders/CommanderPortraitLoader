using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BattleTech;
using BattleTech.Portraits;
using Harmony;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(VersionManifestUtilities), "LoadDefaultManifest")]
    public static class VersionManifestUtilitiesPatch
    {
        public static void Postfix(ref VersionManifest __result)
        {
            try
            {
                string filePath = $"{ CommanderPortraitLoader.ModDirectory}/PortraitJsons/";
                DirectoryInfo d1 = new DirectoryInfo(filePath);
                FileInfo[] f1 = d1.GetFiles("*.json");

                PortraitSettings preset = new PortraitSettings();
                foreach (FileInfo info in f1)
                {
                    using (StreamReader r = new StreamReader(info.FullName))
                    {
                        string json = r.ReadToEnd();
                        preset.FromJSON(json);
                    }
                    __result.AddOrUpdate(preset.Description.Id, info.FullName, "PortraitSettings", DateTime.Now, null, false);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
