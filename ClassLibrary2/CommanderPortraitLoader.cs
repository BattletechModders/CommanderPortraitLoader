using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using System.Reflection;
using System.IO;
using BattleTech.Portraits;
using UnityEngine;
using UnityEngine.Rendering;
using BattleTech.UI;
using HBS;
using HBS.Collections;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(RenderedPortraitResult), "get_Item")]
    public static class RenderedPortraitResult_get_Item_Patch
    {
        static void Postfix(RenderedPortraitResult __instance, ref Texture2D __result)
        {
            Logger.LogLine("RenderedPortraitResult Postfix start");
            if (!string.IsNullOrEmpty(__instance.settings.Description.Icon))
            {
                Logger.LogLine("Icon Found");
                try
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    byte[] array = File.ReadAllBytes("BattleTech_Data/StreamingAssets/sprites/Portraits/" + __instance.settings.Description.Icon + ".png");
                    texture2D.LoadImage(array);
                    __result = texture2D;
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }
    }

    [HarmonyPatch(typeof(SGCharacterCreationWidget), "CreatePilot")]
    public static class SGCharacterCreationWidget_CreatePilot_Patch
    {
        static void Postfix(ref SGCharacterCreationWidget __instance, ref Pilot __result)
        {
            Logger.LogLine("CreatePilot Postfix start");
            try
            {
                if (!string.IsNullOrEmpty(__result.pilotDef.Description.Icon))
                {
                    PilotDef pilotDef = new PilotDef(new HumanDescriptionDef(__result.Description.Id, __result.Description.Callsign, __result.Description.FirstName, __result.Description.LastName,
                        __result.Description.Callsign, __result.Description.Gender, Faction.NoFaction, __result.Description.Age, __result.Description.Details, __result.pilotDef.Description.Icon),
                        __result.Gunnery, __result.Piloting, __result.Guts, __result.Guts, 0, 3, false, 0, string.Empty, CommanderPortraitLoader.GetAbilities(__result.Gunnery, __result.Piloting, __result.Guts, __result.Guts), AIPersonality.Undefined, 0, __result.pilotDef.PilotTags, 0, 0);
                    pilotDef.PortraitSettings = null;
                    pilotDef.SetHiringHallStats(true, false, true, false);
                    __result = new Pilot(pilotDef, "commander", false);
                    Logger.LogLine("Pilot modified");
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(PilotDef), "GetPortraitSprite")]
    public static class PilotDef_GetPortraitSprite_Patch
    {
        static void Postfix(ref PilotDef __instance, ref Sprite __result)
        {
            Logger.LogLine("GetPortraitSprite Postfix start");
            try
            {
                if (__result == null)
                {
                    Texture2D texture2D2 = new Texture2D(2, 2);
                    byte[] data = File.ReadAllBytes("BattleTech_Data/StreamingAssets/sprites/Portraits/" + __instance.Description.Icon + ".png");
                    texture2D2.LoadImage(data);
                    Sprite sprite = new Sprite();
                    sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), new Vector2(0f, 0f), 100f);
                    __result = sprite;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(PilotDef), "GetPortraitSpriteThumb")]
    public static class PilotDef_GetPortraitSpriteThumb_Patch
    {
        static void Postfix(ref PilotDef __instance, ref Sprite __result)
        {
            Logger.LogLine("GetPortraitSpriteThumb Postfix start");
            try
            {
                if (__result == null)
                {
                    Texture2D texture2D2 = new Texture2D(2, 2);
                    byte[] data = File.ReadAllBytes("BattleTech_Data/StreamingAssets/sprites/Portraits/" + __instance.Description.Icon + ".png");
                    texture2D2.LoadImage(data);
                    Sprite sprite = new Sprite();
                    sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), new Vector2(0f, 0f), 100f);
                    __result = CommanderPortraitLoader.DownsampleSprite(sprite);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }

    public static class CommanderPortraitLoader
    {

        public static void Init()
        {
            var harmony = HarmonyInstance.Create("de.morphyum.CommanderPortraitLoader");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }

        // Token: 0x0600564D RID: 22093 RVA: 0x0023F158 File Offset: 0x0023D358
        public static Sprite DownsampleSprite(Sprite oldSprite)
        {
            Texture2D texture = oldSprite.texture;
            RenderTexture temporary = RenderTexture.GetTemporary(texture.width / 2, texture.height / 2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            RenderTexture temporary2 = RenderTexture.GetTemporary(texture.width / 4, texture.height / 4, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            Graphics.Blit(texture, temporary);
            Graphics.Blit(temporary, temporary2);
            Texture2D texture2D = new Texture2D(texture.width / 4, texture.height / 4, TextureFormat.ARGB32, false);
            RenderTexture.active = temporary2;
            texture2D.ReadPixels(new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), 0, 0);
            texture2D.Apply(false, true);
            Texture2D texture2D2 = texture2D;
            texture2D2.name = texture2D2.name + texture.name + " Thumb";
            RenderTexture.ReleaseTemporary(temporary);
            RenderTexture.ReleaseTemporary(temporary2);
            return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, Vector4.zero);
        }

        public static List<string> GetAbilities(int gunnery, int piloting, int guts, int tactics)
        {
            List<string> list = new List<string>();
            SimPilotProgressionConstantsDef progression = LazySingletonBehavior<UnityGameInstance>.Instance.Game.Simulation.Constants.Progression;
            int abilityLevel = gunnery;
            int abilityLevel2 = piloting;
            int abilityLevel3 = guts;
            int abilityLevel4 = tactics;
            list.AddRange(CommanderPortraitLoader.GetAbilityDefsForSkill(progression.GunnerySkills, abilityLevel));
            list.AddRange(CommanderPortraitLoader.GetAbilityDefsForSkill(progression.PilotingSkills, abilityLevel2));
            list.AddRange(CommanderPortraitLoader.GetAbilityDefsForSkill(progression.GutsSkills, abilityLevel3));
            list.AddRange(CommanderPortraitLoader.GetAbilityDefsForSkill(progression.TacticsSkills, abilityLevel4));
            return list;
        }

        public static List<string> GetAbilityDefsForSkill(string[][] abilityDefConsts, int abilityLevel)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < abilityLevel; i++)
            {
                list.AddRange(abilityDefConsts[i]);
            }
            return list;
        }
        /* public static Settings LoadSettings()
         {
             try
             {
                 using (StreamReader r = new StreamReader("mods/CommanderPortraitLoader/settings.json"))
                 {
                     string json = r.ReadToEnd();
                     return JsonConvert.DeserializeObject<Settings>(json);
                 }
             }
             catch (Exception ex)
             {
                 Logger.LogError(ex);
                 return null;
             }
         }*/
    }

    /*public class Settings
     {
         public float RecoveryChance;
     }*/

    public class Logger
    {
        public static void LogError(Exception ex)
        {
            string filePath = "mods/CommanderPortraitLoader/Log.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static void LogLine(String line)
        {
            string filePath = "mods/CommanderPortraitLoader/Log.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(line + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}