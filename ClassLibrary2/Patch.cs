using System;
using Harmony;
using BattleTech;
using System.IO;
using BattleTech.Portraits;
using UnityEngine;
using BattleTech.UI;
using HBS;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;

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
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int stringEmpty = 0;
            int foundIndex = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldsfld)
                {
                    stringEmpty++;
                    if (stringEmpty == 2)
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            if (foundIndex > -1)
            {
                codes[foundIndex].operand = AccessTools.Field(typeof(NewVoice), "newVoice");
            }
            return codes.AsEnumerable();
        }

        static void Postfix(ref SGCharacterCreationWidget __instance, ref Pilot __result) {
            try {
                if (!string.IsNullOrEmpty(__result.pilotDef.PortraitSettings.Description.Icon)) {
                    //Settings settings = Helper.LoadSettings();
                    PilotDef pilotDef = new PilotDef(new HumanDescriptionDef(__result.Description.Id, __result.Description.Callsign, __result.Description.FirstName, __result.Description.LastName,
                        __result.Description.Callsign, __result.Description.Gender, Faction.NoFaction, __result.Description.Age, __result.Description.Details, __result.pilotDef.PortraitSettings.Description.Icon),
                        __result.Gunnery, __result.Piloting, __result.Guts, __result.Guts, 0, 3, false, 0, NewVoice.newVoice, Helper.GetAbilities(__result.Gunnery, __result.Piloting, __result.Guts, __result.Guts), AIPersonality.Undefined, 0, __result.pilotDef.PilotTags, 0, 0);
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

    [HarmonyPatch(typeof(SimGameState), "AddPilotToRoster", new Type[] { typeof(PilotDef), typeof(bool) })]
    public static class SimGameState_AddPilotToRoster_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int foundIndex = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_2)
                {
                    foundIndex = i;
                    break;
                }
            }
            if (foundIndex > -1)
            {
                codes[foundIndex].opcode = OpCodes.Nop;
                codes[foundIndex + 1].opcode = OpCodes.Nop;
            }
            return codes.AsEnumerable();
        }
    }


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

    [HarmonyPatch(typeof(SGCharacterCreationPortraitSelectionPanel), "GetRandomizedSortOrder", new Type[] {typeof(Int32) })]
    public static class SGCharacterCreationPortraitSelectionPanel_PopulateList_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator ilGenerator, IEnumerable<CodeInstruction> instructions)
        {
            int newarrIndex = -1;
            int callvirtIndex = -1;
            int newobjIndex = -1;
            var jump1 = ilGenerator.DefineLabel();
            var jump2 = ilGenerator.DefineLabel();
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Newarr)
                {
                    newarrIndex = i;
                    break;
                }
            }
            for (int j = 0; j < codes.Count; j++)
            {
                if (codes[j].opcode == OpCodes.Callvirt)
                {
                    callvirtIndex = j;
                    break;
                }
            }
            for (int k = 0; k < codes.Count; k++)
            {
                if (codes[k].opcode == OpCodes.Newobj)
                {
                    newobjIndex = k;
                    break;
                }
            }
            codes.Insert(0, new CodeInstruction(OpCodes.Ldarg_1));
            codes.Insert(1, new CodeInstruction(OpCodes.Newarr, codes[newarrIndex + 1].operand));
            codes.Insert(2, new CodeInstruction(OpCodes.Stloc_0));
            codes.Insert(3, new CodeInstruction(OpCodes.Newobj, codes[newobjIndex + 3].operand));
            codes.Insert(4, new CodeInstruction(OpCodes.Stloc_1));
            codes.Insert(5, new CodeInstruction(OpCodes.Ldc_I4_0));
            codes.Insert(6, new CodeInstruction(OpCodes.Stloc_2));
            codes.Insert(7, new CodeInstruction(OpCodes.Br, jump1));

            codes.Insert(8, new CodeInstruction(OpCodes.Ldloc_0) { labels = new List<Label>() { jump2 } });
            codes.Insert(9, new CodeInstruction(OpCodes.Ldloc_2));
            codes.Insert(10, new CodeInstruction(OpCodes.Ldloc_2));
            codes.Insert(11, new CodeInstruction(OpCodes.Stelem_I4));
            codes.Insert(12, new CodeInstruction(OpCodes.Ldloc_1));
            codes.Insert(13, new CodeInstruction(OpCodes.Ldloc_2));
            codes.Insert(14, new CodeInstruction(OpCodes.Callvirt, codes[callvirtIndex + 14].operand));
            codes.Insert(15, new CodeInstruction(OpCodes.Ldloc_2));
            codes.Insert(16, new CodeInstruction(OpCodes.Ldc_I4_1));
            codes.Insert(17, new CodeInstruction(OpCodes.Add));
            codes.Insert(18, new CodeInstruction(OpCodes.Stloc_2));

            codes.Insert(19, new CodeInstruction(OpCodes.Ldloc_2) { labels = new List<Label>() { jump1 } });
            codes.Insert(20, new CodeInstruction(OpCodes.Ldarg_1));
            codes.Insert(21, new CodeInstruction(OpCodes.Blt, jump2));

            codes.Insert(22, new CodeInstruction(OpCodes.Ldloc_0));
            codes.Insert(23, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Array), "Reverse", new Type[] { typeof(Array) })));
            codes.Insert(24, new CodeInstruction(OpCodes.Ldloc_0));
            codes.Insert(25, new CodeInstruction(OpCodes.Ret));
            return codes.AsEnumerable();
        }
    }

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

    [HarmonyPatch(typeof(SGCharacterCreationNamePanel), "LoadOptions")]
    public static class SGCharacterCreationNamePanel_LoadOptions_Patch
    {
        static void Postfix(ref SGCharacterCreationNamePanel __instance)
        {
            __instance.pronounSelector.SetOptions(new string[]
            {
                "Female Pro",
                "Female Pro 2",
                "Female British",
                "Female Asian",
                "Female Irish",
                "Female Midwest",
                "Female Russian",
                "Female Tanesha",
                "Female Tough",
                "Female Bear",
                "Female Creep",
                "Female Kamea",
                "Female Overload",
                "Male Pro",
                "Male British",
                "Male David",
                "Male Brad",
                "Male Rick",
                "Male Matthew",
                "Male Allan",
                "Male Ermy",
                "Male Bear",
                "Male Raju",
                "Male Rizzo",
                "Male Vizzini",
                "Male Overload",
                "Onboard AI"
            });
            __instance.pronounSelector.Select(0);
        }
    }

    [HarmonyPatch(typeof(SGCharacterCreationNamePanel), "get_gender")]
    public static class SGCharacterCreationNamePanel_get_gender_Patch
    {
        public static bool Prefix(ref SGCharacterCreationNamePanel __instance, ref Gender __result)
        {
            return false;
        }
        public static void Postfix(ref SGCharacterCreationNamePanel __instance, ref Gender __result)
        {
            bool lastVOWasLight = false;
            WwiseManager.PostEvent(AudioEventList_vo.vo_stop_pilots, WwiseManager.GlobalAudioObject, null, null);
            NewVoice.newVoice = string.Empty;
            string text = __instance.pronounSelector.selection.ToLower();
            if (text != null)
            {
                if (!(text == "male pro") && !(text == "male british") && !(text == "male david") && !(text == "male brad") && !(text == "male rick") && !(text == "male matthew") && !(text == "male allan") && !(text == "male ermy") && !(text == "male bear") && !(text == "male raju") && !(text == "male rizzo") && !(text == "male vizzini") && !(text == "male overload"))
                {
                    if (!(text == "female pro") && !(text == "female pro 2") && !(text == "female british") && !(text == "female asian") && !(text == "female irish") && !(text == "female midwest") && !(text == "female tanesha") && !(text == "female tough") && !(text == "female bear") && !(text == "female creep") && !(text == "female russian") && !(text == "female kamea") && !(text == "female overload"))
                    {
                        if (text == "onboard ai")
                        {
                            __result = Gender.NonBinary;
                        }
                    }
                    else
                    {
                        if (text == "female pro")
                        {
                            NewVoice.newVoice = "f_pro01";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro01, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female pro 2")
                        {
                            NewVoice.newVoice = "f_pro07";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro07, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female british")
                        {
                            NewVoice.newVoice = "f_pro01_brit";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro01_brit, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female asian")
                        {
                            NewVoice.newVoice = "f_pro02_asian";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro02_asian, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female irish")
                        {
                            NewVoice.newVoice = "f_pro02_irish";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro02_irish, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female midwest")
                        {
                            NewVoice.newVoice = "f_pro4_midwest";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro04_midwest, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female tanesha")
                        {
                            NewVoice.newVoice = "f_pro05_tanesha";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro05_tanesha, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female tough")
                        {
                            NewVoice.newVoice = "f_pro06_tough";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_pro06_tough, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female bear")
                        {
                            NewVoice.newVoice = "f_bear02";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_bear02, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female creep")
                        {
                            NewVoice.newVoice = "f_creep01";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_creep01, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female russian")
                        {
                            NewVoice.newVoice = "f_glum01_russian";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_glum01_russian, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female kamea")
                        {
                            NewVoice.newVoice = "f_kamea";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_kamea, WwiseManager.GlobalAudioObject);
                        }
                        if (text == "female overload")
                        {
                            NewVoice.newVoice = "f_overload01";
                            WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.f_overload01, WwiseManager.GlobalAudioObject);
                        }
                        __result = Gender.Female;
                    }
                }
                else
                {
                    if (text == "male pro")
                    {
                        NewVoice.newVoice = "m_pro01_stark_chad";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro01_stark_chad, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male british")
                    {
                        NewVoice.newVoice = "m_pro03_brit";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro03_brit, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male david")
                    {
                        NewVoice.newVoice = "m_pro02_david";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro02_david, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male brad")
                    {
                        NewVoice.newVoice = "m_pro04_brad";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro04_brad, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male rick")
                    {
                        NewVoice.newVoice = "m_pro05_rick";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro05_rick, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male matthew")
                    {
                        NewVoice.newVoice = "m_pro06_matthew";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro06_matthew, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male allan")
                    {
                        NewVoice.newVoice = "m_pro07_allan";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_pro07_allan, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male ermy")
                    {
                        NewVoice.newVoice = "m_ermy01";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_ermy01, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male bear")
                    {
                        NewVoice.newVoice = "m_bear01";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_bear01, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male raju")
                    {
                        NewVoice.newVoice = "m_raju";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_raju, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male rizzo")
                    {
                        NewVoice.newVoice = "m_rizzo01";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_rizzo01, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male vizzini")
                    {
                        NewVoice.newVoice = "m_vizzini01";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_vizzini01, WwiseManager.GlobalAudioObject);
                    }
                    if (text == "male overload")
                    {
                        NewVoice.newVoice = "m_overload02";
                        WwiseManager.SetSwitch(AudioSwitch_dialog_character_type_pilots.m_overload02, WwiseManager.GlobalAudioObject);
                    }
                    __result = Gender.Male;
                }
                if (NewVoice.newVoice == string.Empty)
                {
                    WwiseManager.SetSwitch(AudioSwitch_dialog_lines_computer_ai.welcome_commander, WwiseManager.GlobalAudioObject);
                    WwiseManager.PostEvent(AudioEventList_vo.vo_play_computer_ai, WwiseManager.GlobalAudioObject, null, null);
                }
                else
                {
                    WwiseManager.SetSwitch(AudioSwitch_dialog_lines_pilots.chosen, WwiseManager.GlobalAudioObject);
                    if (lastVOWasLight)
                    {
                        WwiseManager.SetSwitch(AudioSwitch_dialog_dark_light.dark, WwiseManager.GlobalAudioObject);
                        lastVOWasLight = !lastVOWasLight;
                    }
                    else
                    {
                        WwiseManager.SetSwitch(AudioSwitch_dialog_dark_light.light, WwiseManager.GlobalAudioObject);
                        lastVOWasLight = !lastVOWasLight;
                    }
                    WwiseManager.PostEvent(AudioEventList_vo.vo_play_pilots, WwiseManager.GlobalAudioObject, null, null);
                }
            }
            return;
        }
    }
}
