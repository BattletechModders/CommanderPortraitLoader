using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.Portraits;
using BattleTech.Data;
using UnityEngine;
using System.IO;
using CommanderPortraitLoader;

namespace CommanderPortraitLoader
{
    [HarmonyPatch(typeof(RenderedPortraitResult), "get_Item")]
    class RenderedPortraitResult_GetItemPatch
    {
        static void Postfix(RenderedPortraitResult __instance, ref Texture2D __result)
        {
            if (!string.IsNullOrEmpty(__instance.settings.Description.Icon))
            {
                try
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    foreach (string path in CommanderPortraitLoader.searchablePaths)
                    {
                        string filePath = path + __instance.settings.Description.Icon;
                        byte[] array;
                        if (File.Exists(filePath + ".dds"))
                        {
                            array = File.ReadAllBytes(filePath + ".dds");
                            texture2D = TextureManager.LoadTextureDXT(array);
                            break;
                        }
                        else
                        {
                            array = File.ReadAllBytes(filePath + ".png");
                            texture2D.LoadImage(array);
                            break;
                        }
                    }

                    __result = texture2D;
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }
    }
}
