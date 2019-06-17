using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.Data;
using UnityEngine;

namespace CommanderPortraitLoader
{

    [HarmonyPatch(typeof(PilotDef), "SaveGameRequestResource", typeof(LoadRequest))]
    class PilotDef_SaveGameRequestResourcesPatch
    {
        public static void Postfix(PilotDef __instance, LoadRequest loadRequest)
        {
            if (__instance.PortraitSettings != null)
            {
                if(!string.IsNullOrEmpty(__instance.PortraitSettings.Description.Icon))
                {
                    __instance.Description.SetIcon(__instance.PortraitSettings.Description.Icon);
                    __instance.PortraitSettings = null;
                    Logger.LogLine(string.Format("Loading Pilot: {0}, {1}", (object)__instance.Description.Callsign, (object)__instance.Description.Icon));
                }
            }
            if (!string.IsNullOrEmpty(__instance.Description.Icon))
            {
                Logger.LogLine(string.Format("Loading Pilot: {0}, {1}", (object)__instance.Description.Callsign, (object)__instance.Description.Icon));
                // Issue a Load request for any custom sprites 
                loadRequest.AddBlindLoadRequest(BattleTechResourceType.Sprite, __instance.Description.Icon, new bool?(false));

            }
            else
            {
                Logger.LogLine(string.Format("Got Pilot: {0}", (object)__instance.Description.Callsign));
            }


        }
    }

    [HarmonyPatch(typeof(PilotDef), "GetPortraitSprite")]
    public static class PilotDef_GetPortraitSprite_Patch
    {
        static void Postfix(ref PilotDef __instance, ref Sprite __result)
        {
            if (__result == null)
            {
                Logger.LogLine(string.Format("Got Null Pilot Sprite Request: {0}", (object)__instance.Description.Callsign));
            }
            else
            {
                Logger.LogLine(string.Format("Got Pilot Sprite Request: {0}", (object)__instance.Description.Callsign));
            }
        }
    }

    [HarmonyPatch(typeof(PilotDef), "GetPortraitSpriteThumb")]
    public static class PilotDef_GetPortraitSpriteThumb_Patch
    {
        static void Postfix(ref PilotDef __instance, ref Sprite __result)
        {
            if (__result == null)
            {
                Logger.LogLine(string.Format("Got Null Pilot Thumb Sprite Request: {0}", (object)__instance.Description.Callsign));
            }
            else
            {
                Logger.LogLine(string.Format("Got Pilot Thumb Sprite Request: {0}", (object)__instance.Description.Callsign));
            }
        }
    }
}
