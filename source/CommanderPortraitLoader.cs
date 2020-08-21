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
using BattleTech.Portraits;

namespace CommanderPortraitLoader
{
    public static class CommanderPortraitLoader
    {
        internal static string ModDirectory;
        public static bool disableCreatePilotPatch;
        public static List<CustomVoice> customVoices;
    public static void FinishedLoading(List<string> loadOrder) {
      Logger.LogLine("FinishedLoading");
      try {
        CustomVoiceFetcher.DetectCustomVoices(loadOrder);
        Logger.LogLine("CustomVoices detection status:"+ CustomVoiceFetcher.isCustomVoicesDetected);
        if (CustomVoiceFetcher.isCustomVoicesDetected == false)
        {
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_f_kamea", true));
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_m_raju", true));
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_f_kamea");
            HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_m_raju");
            string[] peristantBanks = new string[5];
            Array.Resize(ref peristantBanks, WwiseDefinitions.PERSISTENT_BANK_IDS.Length + 2);
            for (int index = 0; index < WwiseDefinitions.PERSISTENT_BANK_IDS.Length; ++index)
            {
                peristantBanks[index] = WwiseDefinitions.PERSISTENT_BANK_IDS[index];
            }
            peristantBanks[WwiseDefinitions.PERSISTENT_BANK_IDS.Length] = "vo_f_kamea";
            peristantBanks[WwiseDefinitions.PERSISTENT_BANK_IDS.Length + 1] = "vo_m_raju";
            typeof(WwiseDefinitions).GetField("PERSISTENT_BANK_IDS", BindingFlags.Public | BindingFlags.Static).SetValue(null, peristantBanks);

        }

      } catch (Exception e) {
        Logger.LogError(e);
      }
    }

    public static void Init(string directory, string settingsJSON)
        {
            ModDirectory = directory;
            var harmony = HarmonyInstance.Create("JWolf.CommanderPortraitLoader");
            var original = typeof(PilotRepresentation).GetMethod("PlayPilotVO");
            var genericMethod = original.MakeGenericMethod(new Type[] { typeof(AudioSwitch_dialog_lines_pilots) });
            var transpiler = typeof(PilotRepresentation_PlayPilotVO_Patch).GetMethod("Transpiler");
            harmony.Patch(genericMethod, null, null, new HarmonyMethod(transpiler));
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            disableCreatePilotPatch = true;
            CreateJsons();
        }

        public static void CreateJsons()
        {
            try
            {
                //Create a path for the Json files if it does not already exist
                string jsonPath = $"{ CommanderPortraitLoader.ModDirectory}/../CPLHelper/portraits/";
                Directory.CreateDirectory(jsonPath);

                string filePath = $"{ CommanderPortraitLoader.ModDirectory}/Portraits/";
                DirectoryInfo d1 = new DirectoryInfo(filePath);
                FileInfo[] f1 = d1.GetFiles();
                foreach (FileInfo info in f1)
                {
                    if (info.Name.EndsWith(".png"))
                    {
                        if (!File.Exists(info.FullName.Replace(".png", ".json")))
                        {
                            PortraitSettings portait = new PortraitSettings();
                            portait.headMesh = 0.5f;
                            portait.Randomize(true);
                            portait.Description.SetName(info.Name.Replace(".png", ""));
                            portait.Description.SetID(info.Name.Replace(".png", ""));
                            portait.Description.SetIcon(info.Name.Replace(".png", ""));
                            portait.isCommander = true;
                            using (StreamWriter writer =
                                new StreamWriter(jsonPath + info.Name.Replace(".png", ".json"), false))
                            {
                                writer.WriteLine(portait.ToJSON());
                            }
                        }
                    }
                    else
                    {
                        if (info.Name.EndsWith(".dds"))
                        {
                            if (!File.Exists(info.FullName.Replace(".dds", ".json")))
                            {
                                PortraitSettings portait = new PortraitSettings();
                                portait.headMesh = 0.5f;
                                portait.Randomize(true);
                                portait.Description.SetName(info.Name.Replace(".dds", ""));
                                portait.Description.SetID(info.Name.Replace(".dds", ""));
                                portait.Description.SetIcon(info.Name.Replace(".dds", ""));
                                portait.isCommander = true;
                                using (StreamWriter writer =
                                    new StreamWriter(jsonPath + info.Name.Replace(".dds", ".json"), false))
                                {
                                    writer.WriteLine(portait.ToJSON());
                                }
                            }
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
