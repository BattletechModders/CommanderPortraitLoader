using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleTech.UI;
using Harmony;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SGCharacterCreationCareerBackgroundSelectionPanel), "UpdatePilotSummary")]
    public static class SGCharacterCreationCareerBackgroundSelectionPanel_UpdatePilotSummary
    {
        static void Prefix()
        {
            CommanderPortraitLoader.disableCreatePilotPatch = true;
        }
        static void Postfix()
        {
            CommanderPortraitLoader.disableCreatePilotPatch = false;
        }

    }
}
