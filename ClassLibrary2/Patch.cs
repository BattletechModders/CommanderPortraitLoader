using System;
using Harmony;
using BattleTech;
using System.IO;
using BattleTech.Portraits;
using UnityEngine;
using BattleTech.UI;

namespace CommanderPortraitLoader {
    [HarmonyPatch(typeof(RenderedPortraitResult), "get_Item")]
    public static class RenderedPortraitResult_get_Item_Patch {
        static void Postfix(RenderedPortraitResult __instance, ref Texture2D __result) {
            Logger.LogLine("RenderedPortraitResult Postfix start");
            if (!string.IsNullOrEmpty(__instance.settings.Description.Icon)) {
                Logger.LogLine("Icon Found");
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
            Logger.LogLine("CreatePilot Postfix start");
            try {
                if (!string.IsNullOrEmpty(__result.pilotDef.Description.Icon)) {
                    PilotDef pilotDef = new PilotDef(new HumanDescriptionDef(__result.Description.Id, __result.Description.Callsign, __result.Description.FirstName, __result.Description.LastName,
                        __result.Description.Callsign, __result.Description.Gender, Faction.NoFaction, __result.Description.Age, __result.Description.Details, __result.pilotDef.Description.Icon),
                        __result.Gunnery, __result.Piloting, __result.Guts, __result.Guts, 0, 3, false, 0, string.Empty, Helper.GetAbilities(__result.Gunnery, __result.Piloting, __result.Guts, __result.Guts), AIPersonality.Undefined, 0, __result.pilotDef.PilotTags, 0, 0);
                    pilotDef.PortraitSettings = null;
                    pilotDef.SetHiringHallStats(true, false, true, false);
                    __result = new Pilot(pilotDef, "commander", false);
                    Logger.LogLine("Pilot modified");
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
            Logger.LogLine("GetPortraitSprite Postfix start");
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
            Logger.LogLine("GetPortraitSpriteThumb Postfix start");
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

    /*[HarmonyPatch(typeof(SGCharacterCreationPortraitSelectionPanel), "PopulateList")]
    public static class SGCharacterCreationPortraitSelectionPanel_PopulateList_Patch {
        static void Postfix(SGCharacterCreationPortraitSelectionPanel __instance) {
            Logger.LogLine("PopulateList Prefix start");

            try {
                PortraitSettings settings = new PortraitSettings();
                string json;
                using (StreamReader r = new StreamReader("mods/CommanderPortraitLoader/Portraits/PortraitPreset_Kara.json")) {
                    json = r.ReadToEnd();
                }
                settings.FromJSON(json);
                DataManager data = LazySingletonBehavior<UnityGameInstance>.Instance.Game.DataManager;
                KeyValuePair<string, PortraitSettings> pair = new KeyValuePair<string, PortraitSettings>(settings.Description.Id, settings);
                data.PortraitSettings.Add(pair);
            }
            catch (Exception e) {

                Logger.LogError(e);
            }
        }
    }*/

    [HarmonyPatch(typeof(VersionManifestUtilities), "LoadDefaultManifest")]
    public static class VersionManifestUtilitiesPatch {
        // ReSharper disable once RedundantAssignment
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
}