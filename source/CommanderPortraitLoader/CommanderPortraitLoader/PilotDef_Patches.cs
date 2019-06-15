using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.Data;

namespace CommanderPortraitLoader
{

    [HarmonyPatch(typeof(PilotDef), "SaveGameRequestResource", typeof(LoadRequest))]
    class PilotDef_SaveGameRequestResourcesPatch
    {
        public static void Postfix(PilotDef __instance, LoadRequest loadRequest)
        {
            if (!string.IsNullOrEmpty(__instance.Description.Icon))
            {
                Logger.LogLine(string.Format("Loading Pilot: {0}", (object)__instance.Description.Callsign));
                loadRequest.AddBlindLoadRequest(BattleTechResourceType.Sprite, __instance.Description.Icon, new bool?(false));

            }


        }
    }
}
