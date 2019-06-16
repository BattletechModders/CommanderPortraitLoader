using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.UI;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SGCharacterCreationWidget), "CreatePilot")]
    public static class SGCharacterCreationWidget_CreatePilotPatch
    {
        static void Postfix(ref SGCharacterCreationWidget __instance, ref Pilot __result)
        {
            try
            {
                if (!string.IsNullOrEmpty(__result.pilotDef.PortraitSettings.Description.Icon))
                {
                    __result.pilotDef.Description.SetIcon(__result.pilotDef.PortraitSettings.Description.Icon);
                    __result.pilotDef.PortraitSettings = null;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

    }
}
