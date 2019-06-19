using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using BattleTech;

namespace CommanderPortraitLoader
{
    public static class PilotRepresentation_PlayPilotVO_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int foundIndex = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Callvirt && (codes[i].operand as MethodInfo)?.Name == "get_IsPlayerCharacter")
                {
                    foundIndex = i;
                    break;
                }
            }
            if (foundIndex > -1)
            {
                codes.Insert(foundIndex + 2, new CodeInstruction(OpCodes.Ldarg_0));
                codes.Insert(foundIndex + 3, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PilotRepresentation), "pilot")));
                codes.Insert(foundIndex + 4, new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Pilot), "get_pilotDef")));
                codes.Insert(foundIndex + 5, new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PilotDef), "get_Voice")));
                codes.Insert(foundIndex + 6, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(String), "IsNullOrEmpty", new Type[] { typeof(string) })));
                codes.Insert(foundIndex + 7, codes[foundIndex + 1]);
            }
            return codes.AsEnumerable();
        }
    }
}
