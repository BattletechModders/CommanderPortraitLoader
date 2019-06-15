using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using System.Reflection;

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
            //CreateJsons();
            //AddOrUpdateJSONToManifest();
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_f_kamea", true));
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.LoadBank((AudioBankList)Enum.Parse(typeof(AudioBankList), "vo_m_raju", true));
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_f_kamea");
            //HBS.SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add("vo_m_raju");
            //HarmonyInstance.DEBUG = true;
        }
    }
}
