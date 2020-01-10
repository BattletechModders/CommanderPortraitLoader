using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.UI;
using BattleTech.Data;

namespace CommanderPortraitLoader {
  [HarmonyPatch(typeof(SGCharacterCreationWidget), "CreatePilot")]
  public static class SGCharacterCreationWidget_CreatePilotPatch {
    static void Postfix(ref SGCharacterCreationWidget __instance, ref Pilot __result) {
      if (CustomVoiceFetcher.isCustomVoicesDetected) { return; }

      if (!string.IsNullOrEmpty(NewVoice.newVoice)) {
        __result.pilotDef.SetVoice(NewVoice.newVoice);
      }
    }
  }
}
