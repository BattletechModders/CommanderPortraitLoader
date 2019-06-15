using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommanderPortraitLoader
{
    public static class CommanderPortraitLoader
    {
        internal static string ModDirectory;

        public static void Init(string directory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("JWolf.CommanderPortraitLoader");
            var original = typeof(PilotRepresentation).GetMethod("PlayPilotVO");
            //var genericMethod = original.MakeGenericMethod(new Type[] { typeof(AudioSwitch_dialog_lines_pilots) });
            //var transpiler = typeof(PilotRepresentation_PlayPilotVO_Patch).GetMethod("Transpiler");
            //harmony.Patch(genericMethod, null, null, new HarmonyMethod(transpiler));
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModDirectory = directory;
            CreateJsons();
            //AddOrUpdateJSONToManifest();
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_f_kamea", true));
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_m_raju", true));
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_f_kamea");
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_m_raju");
            //HarmonyInstance.DEBUG = true;
        }

        public static void CreateJsons()
        {
            try
            {
                //Create a path for the Json files if it does not already exist
                string jsonPath = $"{ CommanderPortraitLoader.ModDirectory}/PortraitJsons/";
                Directory.CreateDirectory(jsonPath);

                string filePath = $"{ CommanderPortraitLoader.ModDirectory}/Portraits/";
                DirectoryInfo d1 = new DirectoryInfo(filePath);
                FileInfo[] f1 = d1.GetFiles("*.png");
                foreach (FileInfo info in f1)
                {
                    if (!File.Exists(info.FullName.Replace(".png", ".json")))
                    {
                        CustomPreset preset = new CustomPreset();
                        preset.isCommander = true;
                        preset.Description = new CustomDescription();
                        preset.Description.Id = info.Name.Replace(".png", "");
                        preset.Description.Icon = info.Name.Replace(".png", "");
                        preset.Description.Name = info.Name.Replace(".png", "");
                        preset.Description.Details = "";
                        JObject o = (JObject)JToken.FromObject(preset);
                        using (StreamWriter writer = new StreamWriter(jsonPath + info.Name.Replace(".png", ".json"), false))
                        {
                            writer.WriteLine(o);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
