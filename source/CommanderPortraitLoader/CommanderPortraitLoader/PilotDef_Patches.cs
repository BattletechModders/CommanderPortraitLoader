using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.Data;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(PilotDef), "DependenciesLoaded", typeof(uint))]
    class PilotDef_DependenciesLoadedPatch
    {
        public static void Postfix(PilotDef __instance, uint loadWeight)
        {
            if (!string.IsNullOrEmpty(__instance.Description.Icon))
            {
                bool hasIcon = __instance.DataManager.Exists(BattleTechResourceType.Sprite, __instance.Description.Icon);
                Logger.LogLine(string.Format("Checking Pilot: {0}, Icon Exists: {1}", (object)__instance.Description.Callsign, (object)hasIcon));

            }


        }
    }

    [HarmonyPatch(typeof(PilotDef), "GatherDependencies", typeof(DataManager), typeof(DataManager.DependencyLoadRequest), typeof(uint))]
    class PilotDef_GatherDependenciesPatch
    {
        public static void Postfix(PilotDef __instance, DataManager dataManager, DataManager.DependencyLoadRequest dependencyLoad, uint activeRequestWeight)
        {
            if (!string.IsNullOrEmpty(__instance.Description.Icon))
            {
                Logger.LogLine(string.Format("Gathering Pilot: {0}", (object)__instance.Description.Callsign));

            }


        }
    }

    [HarmonyPatch(typeof(PilotDef), "SaveGameRequestResource", typeof(LoadRequest))]
    class PilotDef_SaveGameRequestResourcesPatch
    {
        public static void Postfix(PilotDef __instance, LoadRequest loadRequest)
        {
            if (!string.IsNullOrEmpty(__instance.Description.Icon))
            {
                Logger.LogLine(string.Format("Loading Pilot: {0}", (object)__instance.Description.Callsign));

            }


        }
    }
}
