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

    [HarmonyPatch(typeof(SGCharacterCreationWidget), "ReceiveButtonPress")]
    public static class SSGCharacterCreationWidget_ReceiveButtonPressPatch
    {
        static void Prefix()
        {
            CommanderPortraitLoader.disableCreatePilotPatch = false;
        }
        static void Postfix()
        {
            CommanderPortraitLoader.disableCreatePilotPatch = true;
        }

    }
}
