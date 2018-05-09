using System;
using Harmony;
using BattleTech;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CommanderPortraitLoader {
    public static class CommanderPortraitLoader {

        public static void Init() {
            var harmony = HarmonyInstance.Create("de.morphyum.CommanderPortraitLoader");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            CreateJsons();
            AddOrUpdateJSONToManifest();
        }

        public static void CreateJsons() {
            try {
                string filePath = "mods/CommanderPortraitLoader/Portraits/";
                DirectoryInfo d1 = new DirectoryInfo(filePath);
                FileInfo[] f1 = d1.GetFiles("*.png");
                foreach (FileInfo info in f1) {
                    if (!File.Exists(info.FullName.Replace(".png", ".json"))) {
                        CustomPreset preset = new CustomPreset();
                        preset.isCommander = true;
                        preset.Description = new CustomDescription();
                        preset.Description.Id = info.Name.Replace(".png", "");
                        preset.Description.Icon = info.Name.Replace(".png", "");
                        preset.Description.Name = info.Name.Replace(".png", "");
                        preset.Description.Details = "";
                        JObject o = (JObject)JToken.FromObject(preset);
                        using (StreamWriter writer = new StreamWriter(filePath + info.Name.Replace(".png", ".json"), false)) {
                            writer.WriteLine(o);
                        }
                    }
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }

        private static void AddOrUpdateJSONToManifest() {
            try {
                string filePath = "mods/CommanderPortraitLoader/Portraits/";
                VersionManifest manifest = VersionManifestUtilities.ManifestFromCSV("mods/CommanderPortraitLoader/VersionManifest.csv");
                DirectoryInfo d1 = new DirectoryInfo(filePath);
                FileInfo[] f1 = d1.GetFiles("*.png");
                foreach (VersionManifestEntry entry in manifest.Entries) {
                    if (!File.Exists(entry.FilePath.Replace(".json", ".png"))) {
                        if (File.Exists(entry.FilePath)) {
                            File.Delete(entry.FilePath);
                        }
                        manifest.Remove(entry.Id, entry.Type, DateTime.Now);
                        manifest.ClearRemoved();
                    }
                }
                f1 = d1.GetFiles("*.json");
                CustomPreset preset = new CustomPreset();
                foreach (FileInfo info in f1) {
                    using (StreamReader r = new StreamReader(info.FullName)) {
                        string json = r.ReadToEnd();
                        preset = JsonConvert.DeserializeObject<CustomPreset>(json);
                    }
                    manifest.AddOrUpdate(preset.Description.Id, info.FullName, "PortraitSettings", DateTime.Now, null, false);
                }
                VersionManifestUtilities.ManifestToCSV(manifest, "mods/CommanderPortraitLoader/VersionManifest.csv");
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }

        /* public static Settings LoadSettings()
         {
             try
             {
                 using (StreamReader r = new StreamReader("mods/CommanderPortraitLoader/settings.json"))
                 {
                     string json = r.ReadToEnd();
                     return JsonConvert.DeserializeObject<Settings>(json);
                 }
             }
             catch (Exception ex)
             {
                 Logger.LogError(ex);
                 return null;
             }
         }*/
    }

    /*public class Settings
     {
         public float RecoveryChance;
     }*/

}