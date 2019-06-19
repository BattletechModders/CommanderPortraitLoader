using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using HBS;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SpawnableUnit), "Init", new Type[] { typeof(PilotableActorDef), typeof(PilotDef) })]
    public static class SpawnableUnit_Init_Patch
    {
        static void Prefix(ref SpawnableUnit __instance, PilotDef pilotDef)
        {
            try
            {
                if (!string.IsNullOrEmpty(pilotDef.Voice))
                {
                    string text2 = string.Format("vo_{0}", pilotDef.Voice);
                    AudioBankList bankId2 = (AudioBankList)Enum.Parse(typeof(AudioBankList), text2, true);
                    SceneSingletonBehavior<WwiseManager>.Instance.LoadBank(bankId2);
                    SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add(text2);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
