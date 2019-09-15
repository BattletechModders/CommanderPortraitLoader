using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleTech.UI;
using Harmony;
using BattleTech;
using System.Reflection.Emit;
using System.Reflection;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SGBarracksDossierPanel), "PlayPilotSelectionVO", new Type[] { typeof(Pilot) })]
    public static class SGBarracksDossierPanel_PlayPilotSelectionVO_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int startIndex = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Callvirt && (codes[i].operand as MethodInfo)?.Name == "get_IsPlayerCharacter")
                {
                    startIndex = i;
                    break;
                }
            }
            if (startIndex > -1)
            {
                codes.RemoveRange(startIndex - 1, 3);
            }
            return codes.AsEnumerable();
        }
    }
}
