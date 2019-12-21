using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.UI;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(SGCharacterCreationNamePanel), "LoadOptions")]
    public static class SGCharacterCreationNamePanel_LoadOptions_Patch
    {
        static bool Prefix()
        {
            return false;
        }

        static void Postfix(ref SGCharacterCreationNamePanel __instance)
        {
            CommanderPortraitLoader.customVoices = CustomVoiceFetcher.GetCustomVoices();
            List<string> sOptions = new List<string>();
            sOptions.Add("Female Pro");
            sOptions.Add("Female Pro 2");
            sOptions.Add("Female British");
            sOptions.Add("Female Asian");
            sOptions.Add("Female Irish");
            sOptions.Add("Female Midwest");
            sOptions.Add("Female Russian");
            sOptions.Add("Female Tanesha");
            sOptions.Add("Female Tough");
            sOptions.Add("Female Bear");
            sOptions.Add("Female Creep");
            sOptions.Add("Female Kamea");
            sOptions.Add("Female Overload");

            sOptions.Add("Male Pro");
            sOptions.Add("Male British");
            sOptions.Add("Male David");
            sOptions.Add("Male Brad");
            sOptions.Add("Male Rick");
            sOptions.Add("Male Matthew");
            sOptions.Add("Male Allan");
            sOptions.Add("Male Ermy");
            sOptions.Add("Male Bear");
            sOptions.Add("Male Raju");
            sOptions.Add("Male Rizzo");
            sOptions.Add("Male Vizzini");
            sOptions.Add("Male Overload");

            sOptions.Add("Onboard AI");

            foreach (CustomVoice cv in CommanderPortraitLoader.customVoices)
            {
                sOptions.Add(cv.name);
            }

            ReflectionHelper.SetPrivateField(__instance, "nameWasBlank", (bool)ReflectionHelper.InvokePrivateMethod(__instance, "get_NameIsBlank", null));
            __instance.pronounSelector.SetOptions(sOptions.ToArray());
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
                if (CommanderPortraitLoader.customVoices != null)
                {
                    foreach (CustomVoice cv in CommanderPortraitLoader.customVoices)
                    {
                        if (text == cv.name)
                        {
                            NewVoice.newVoice = cv.name;
                            __result = cv.gender;
                            SGBarracksDossierPanel.PlayVO(cv.name);
                            return;
                        }
                    }
                }
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
                            NewVoice.newVoice = "f_pro04_midwest";
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
