using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.UI;
using BattleTech.Data;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SGCharacterCreationWidget), "CreatePilot")]
    public static class SGCharacterCreationWidget_CreatePilotPatch
    {
        static void Postfix(ref SGCharacterCreationWidget __instance, ref Pilot __result, SimGameState ___SimGame)
        {
            if (!CommanderPortraitLoader.disableCreatePilotPatch)
            {
                // This is dead code right now, and not needed for portraits, 
                // leaving it here because I'll probably need to patch this method for audio
                try
                {
                    if (!string.IsNullOrEmpty(__result.pilotDef.PortraitSettings.Description.Icon))
                    {
                        __result.pilotDef.Description.SetIcon(__result.pilotDef.PortraitSettings.Description.Icon);
                        __result.pilotDef.PortraitSettings = null;
                        LoadRequest loadRequest = ___SimGame.DataManager.CreateLoadRequest((Action<LoadRequest>)null, false);
                        loadRequest.AddBlindLoadRequest(BattleTechResourceType.Sprite, __result.pilotDef.Description.Icon);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }

    }

}
