﻿using System;
using Harmony;
using BattleTech;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CommanderPortraitLoader {
    public static class CommanderPortraitLoader {
        internal static string ModDirectory;

        public static void Init(string directory, string settingsJSON) {
            var harmony = HarmonyInstance.Create("de.morphyum.CommanderPortraitLoader");
            var original = typeof(PilotRepresentation).GetMethod("PlayPilotVO");
            var genericMethod = original.MakeGenericMethod(new Type[] { typeof(AudioSwitch_dialog_lines_pilots) });
            var transpiler = typeof(PilotRepresentation_PlayPilotVO_Patch).GetMethod("Transpiler");
            harmony.Patch(genericMethod, null, null, new HarmonyMethod(transpiler));
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModDirectory = directory;
            CreateJsons();
            AddOrUpdateJSONToManifest();
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_f_kamea", true));
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_m_raju", true));
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_f_kamea");
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_m_raju");
            //HarmonyInstance.DEBUG = true;
        }

        public static void CreateJsons() {
            try {
                string filePath = $"{ CommanderPortraitLoader.ModDirectory}/Portraits/";
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
                string filePath = $"{ CommanderPortraitLoader.ModDirectory}/Portraits/";
                VersionManifest manifest = VersionManifestUtilities.ManifestFromCSV($"{ CommanderPortraitLoader.ModDirectory}/VersionManifest.csv");
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
                VersionManifestUtilities.ManifestToCSV(manifest, $"{ CommanderPortraitLoader.ModDirectory}/VersionManifest.csv");
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

}