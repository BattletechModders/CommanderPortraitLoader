using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.UI;
using System.Reflection;
using System.Reflection.Emit;

namespace CommanderPortraitLoader
{


    [HarmonyPatch(typeof(SGBarracksMWDetailPanel), "CustomizePilot")]
    public static class SGBarracksMWDetailPanel_CustomizePilotPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int startIndex = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldfld)
                {
                    startIndex = i;
                    break;
                }
            }
            if (startIndex > -1)
            {
                codes.RemoveRange(startIndex + 1, 8);
                codes.Insert(startIndex + 1, new CodeInstruction(OpCodes.Ldc_I4_0));
            }
            return codes.AsEnumerable();
        }
    }
}
