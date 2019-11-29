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
                // perform this here on the first save load after character creation
                // this avoids all sorts of exceptions and problems with the character customization UIs
                // this also means we only need to do this in one patch instead of many
                if(!string.IsNullOrEmpty(__instance.PortraitSettings.Description.Icon))
                {
                    __instance.Description.SetIcon(__instance.PortraitSettings.Description.Icon);
                    __instance.PortraitSettings = null;
                    Logger.LogLine(string.Format("Applying Hardset Icon to Pilot: {0}, {1}", (object)__instance.Description.Callsign, (object)__instance.Description.Icon));
                }
            }
            if (!string.IsNullOrEmpty(__instance.Description.Icon))
            {
                //Logger.LogLine(string.Format("Loading Pilot: {0}, {1}", (object)__instance.Description.Callsign, (object)__instance.Description.Icon));
                // Issue a Load request for any custom sprites 
                try
                {
                    Logger.LogLine(string.Format("Issuing  Load Request Icon for Pilot: {0}, {1}", (object)__instance.Description.Callsign, (object)__instance.Description.Icon));
                    loadRequest.AddBlindLoadRequest(BattleTechResourceType.Sprite, __instance.Description.Icon, new bool?(false));
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }

            }

        }
    }

}
