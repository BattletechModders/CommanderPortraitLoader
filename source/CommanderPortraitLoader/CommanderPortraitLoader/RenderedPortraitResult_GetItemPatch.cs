using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.Portraits;
using UnityEngine;
using System.IO;

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
                    byte[] array = File.ReadAllBytes($"{ CommanderPortraitLoader.ModDirectory}/Portraits/" + __instance.settings.Description.Icon + ".png");
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
}
