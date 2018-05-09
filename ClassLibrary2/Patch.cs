using System;
using Harmony;
using BattleTech;
using System.IO;
using BattleTech.Portraits;
using UnityEngine;
using BattleTech.UI;
using HBS;

namespace CommanderPortraitLoader {
    [HarmonyPatch(typeof(RenderedPortraitResult), "get_Item")]
    public static class RenderedPortraitResult_get_Item_Patch {
        static void Postfix(RenderedPortraitResult __instance, ref Texture2D __result) {
            if (!string.IsNullOrEmpty(__instance.settings.Description.Icon)) {
                try {
                    Texture2D texture2D = new Texture2D(2, 2);
                    byte[] array = File.ReadAllBytes("mods/CommanderPortraitLoader/Portraits/" + __instance.settings.Description.Icon + ".png");
                    texture2D.LoadImage(array);
                    __result = texture2D;
                }
                catch (Exception e) {
                    Logger.LogError(e);
                }
            }
        }
    }

    [HarmonyPatch(typeof(SGCharacterCreationWidget), "CreatePilot")]
    public static class SGCharacterCreationWidget_CreatePilot_Patch {
        static void Postfix(ref SGCharacterCreationWidget __instance, ref Pilot __result) {
            try {

                if (!string.IsNullOrEmpty(__result.pilotDef.PortraitSettings.Description.Icon)) {
                    Settings settings = Helper.LoadSettings();
                    PilotDef pilotDef = new PilotDef(new HumanDescriptionDef(__result.Description.Id, __result.Description.Callsign, __result.Description.FirstName, __result.Description.LastName,
                        __result.Description.Callsign, __result.Description.Gender, Faction.NoFaction, __result.Description.Age, __result.Description.Details, __result.pilotDef.PortraitSettings.Description.Icon),
                        __result.Gunnery, __result.Piloting, __result.Guts, __result.Guts, 0, 3, false, 0, settings.newCommanderVoice, Helper.GetAbilities(__result.Gunnery, __result.Piloting, __result.Guts, __result.Guts), AIPersonality.Undefined, 0, __result.pilotDef.PilotTags, 0, 0);
                    pilotDef.PortraitSettings = null;
                    pilotDef.SetHiringHallStats(true, false, true, false);
                    __result = new Pilot(pilotDef, "commander", false);
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(PilotDef), "GetPortraitSprite")]
    public static class PilotDef_GetPortraitSprite_Patch {
        static void Postfix(ref PilotDef __instance, ref Sprite __result) {
            try {
                if (__result == null) {
                    Texture2D texture2D2 = new Texture2D(2, 2);
                    byte[] data = File.ReadAllBytes("mods/CommanderPortraitLoader/Portraits/" + __instance.Description.Icon + ".png");
                    texture2D2.LoadImage(data);
                    Sprite sprite = new Sprite();
                    sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), new Vector2(0f, 0f), 100f);
                    __result = sprite;
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(PilotDef), "GetPortraitSpriteThumb")]
    public static class PilotDef_GetPortraitSpriteThumb_Patch {
        static void Postfix(ref PilotDef __instance, ref Sprite __result) {
            try {
                if (__result == null) {
                    Texture2D texture2D2 = new Texture2D(2, 2);
                    byte[] data = File.ReadAllBytes("mods/CommanderPortraitLoader/Portraits/" + __instance.Description.Icon + ".png");
                    texture2D2.LoadImage(data);
                    Sprite sprite = new Sprite();
                    sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), new Vector2(0f, 0f), 100f);
                    __result = Helper.DownsampleSprite(sprite);
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(VersionManifestUtilities), "LoadDefaultManifest")]
    public static class VersionManifestUtilitiesPatch {
        public static void Postfix(ref VersionManifest __result) {
            try {
                var addendum = VersionManifestUtilities.ManifestFromCSV("mods/CommanderPortraitLoader/VersionManifest.csv");
                foreach (var entry in addendum.Entries) {
                    __result.AddOrUpdate(entry.Id, entry.FilePath, entry.Type, entry.AddedOn, entry.AssetBundleName, entry.IsAssetBundlePersistent);
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }

        }
    }

    [HarmonyPatch(typeof(SpawnableUnit), "Init", new Type[] { typeof(PilotableActorDef), typeof(PilotDef) })]
    public static class SpawnableUnit_Init_Patch {
        static void Prefix(ref SpawnableUnit __instance, PilotDef pilotDef) {
            try {
                if (!string.IsNullOrEmpty(pilotDef.Voice)) {
                    string text2 = string.Format("vo_{0}", pilotDef.Voice);
                    AudioBankList bankId2 = (AudioBankList)Enum.Parse(typeof(AudioBankList), text2, true);
                    SceneSingletonBehavior<WwiseManager>.Instance.LoadBank(bankId2);
                    SceneSingletonBehavior<WwiseManager>.Instance.voBanks.Add(text2);
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }


    public static class PilotRepresentation_PlayPilotVO_Patch {
        public static void Prefix(PilotRepresentation __instance, ref bool __state) {
            try {
                Logger.LogLine("Voice: " + __instance.pilot.pilotDef.Voice);
                if (__instance.pilot.IsPlayerCharacter) {
                    Logger.LogLine("Player Prefix"); 
                    if (!string.IsNullOrEmpty(__instance.pilot.pilotDef.Voice)) {
                        Logger.LogLine("Prefix If");
                        __state = __instance.pilot.pilotDef.PilotTags.Remove("commander_player");
                        Logger.LogLine(__state.ToString());
                    }
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }

        }

        public static void Postfix(PilotRepresentation __instance, bool __state) {
            try {
                if (__state) {
                    Logger.LogLine("Postfix If");
                    __instance.pilot.pilotDef.PilotTags.Add("commander_player");
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }

        }
    }


}
