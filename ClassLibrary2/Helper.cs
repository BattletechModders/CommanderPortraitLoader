using System.Collections.Generic;
using BattleTech;
using UnityEngine;
using HBS;
using System.IO;
using Newtonsoft.Json;
using System;

namespace CommanderPortraitLoader {
    public class Helper{
        // Token: 0x0600564D RID: 22093 RVA: 0x0023F158 File Offset: 0x0023D358
        public static Sprite DownsampleSprite(Sprite oldSprite) {
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

        public static List<string> GetAbilities(int gunnery, int piloting, int guts, int tactics) {
            List<string> list = new List<string>();
            SimPilotProgressionConstantsDef progression = LazySingletonBehavior<UnityGameInstance>.Instance.Game.Simulation.Constants.Progression;
            int abilityLevel = gunnery;
            int abilityLevel2 = piloting;
            int abilityLevel3 = guts;
            int abilityLevel4 = tactics;
            list.AddRange(GetAbilityDefsForSkill(progression.GunnerySkills, abilityLevel));
            list.AddRange(GetAbilityDefsForSkill(progression.PilotingSkills, abilityLevel2));
            list.AddRange(GetAbilityDefsForSkill(progression.GutsSkills, abilityLevel3));
            list.AddRange(GetAbilityDefsForSkill(progression.TacticsSkills, abilityLevel4));
            return list;
        }

        public static List<string> GetAbilityDefsForSkill(string[][] abilityDefConsts, int abilityLevel) {
            List<string> list = new List<string>();
            for (int i = 0; i < abilityLevel; i++) {
                list.AddRange(abilityDefConsts[i]);
            }
            return list;
        }

        public static Settings LoadSettings() {
            try {
                using (StreamReader r = new StreamReader("mods/CommanderPortraitLoader/settings.json")) {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
                return null;
            }
        }
    }
}