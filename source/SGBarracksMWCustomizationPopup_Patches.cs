﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Harmony;
using BattleTech;
using BattleTech.UI;
using BattleTech.Portraits;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SGBarracksMWCustomizationPopup), "Save")]
    public static class SGBarracksMWCustomizationPopup_SavePatch
    {
        static void Postfix(ref SGBarracksMWCustomizationPopup __instance)
        {
            if (!string.IsNullOrEmpty(NewVoice.newVoice))
            {
                __instance.pilot.pilotDef.SetVoice(NewVoice.newVoice);
            }
            if (!string.IsNullOrEmpty(__instance.pilot.pilotDef.Description.Icon))
            {
                __instance.pilot.pilotDef.PortraitSettings = null;
            }
        }
    }

    [HarmonyPatch(typeof(SGBarracksMWCustomizationPopup), "LoadPortraitSettings")]
    public static class SGBarracksMWCustomizationPopup_LoadPortraitSettingsPatch
    {
        static void Prefix(ref SGBarracksMWCustomizationPopup __instance, ref PortraitSettings portraitSettingsData)
        {
            try
            {
                if (portraitSettingsData == null)
                {
                    if(!string.IsNullOrEmpty(__instance.pilot.pilotDef.Description.Icon))
                    {
                        string filePath = $"{ CommanderPortraitLoader.ModDirectory}/PortraitJsons/portraits/" + __instance.pilot.pilotDef.Description.Icon + ".json";
                        if (File.Exists(filePath))
                        {
                            portraitSettingsData = new PortraitSettings();
                            using (StreamReader r = new StreamReader(filePath))
                            {
                                string json = r.ReadToEnd();
                                portraitSettingsData.FromJSON(json);
                            }
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
