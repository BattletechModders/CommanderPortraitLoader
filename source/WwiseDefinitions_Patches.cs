using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using Harmony;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(WwiseDefinitions), "PersistentBanksContains", new Type[] { typeof(string) })]
    public static class WwiseDefinitions_PersistentBankContains_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator ilGenerator, IEnumerable<CodeInstruction> instructions)
        {
            var fail = ilGenerator.DefineLabel();
            var success = ilGenerator.DefineLabel();
            var codes = new List<CodeInstruction>(instructions);
            codes.Insert(0, new CodeInstruction(OpCodes.Ldstr, "vo_m_raju"));
            codes.Insert(1, new CodeInstruction(OpCodes.Ldarg_0));
            codes.Insert(2, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(String), "op_Equality", new Type[] { typeof(string), typeof(string) })));
            codes.Insert(3, new CodeInstruction(OpCodes.Brtrue, success));
            codes.Insert(4, new CodeInstruction(OpCodes.Ldstr, "vo_f_kamea"));
            codes.Insert(5, new CodeInstruction(OpCodes.Ldarg_0));
            codes.Insert(6, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(String), "op_Equality", new Type[] { typeof(string), typeof(string) })));
            codes.Insert(7, new CodeInstruction(OpCodes.Brfalse, fail));
            codes.Insert(8, new CodeInstruction(OpCodes.Ldc_I4_1) { labels = new List<Label>() { success } });
            codes.Insert(9, new CodeInstruction(OpCodes.Ret));
            codes.Insert(10, new CodeInstruction(OpCodes.Nop) { labels = new List<Label>() { fail } });
            return codes.AsEnumerable();
        }
    }
}
