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
using IRBTModUtils;

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
                    Texture2D texture2D = null;
                    Logger.LogLine("RenderedPortraitResult.Item "+ __instance.settings.Description.Icon);
                    foreach (string path in CommanderPortraitLoader.searchablePaths)
                    {
                        string[] files = Directory.GetFiles(path, __instance.settings.Description.Icon + ".*");
                        foreach (string filePath in files) {
                          //string filePath = Path.Combine(path, __instance.settings.Description.Icon);
                          foreach (string ext in CommanderPortraitLoader.supportedSuffixes) {
                              if (filePath.EndsWith(ext)) {
                                  byte[] data = File.ReadAllBytes(filePath);
                                  if (TextureManager.IsDDS(data)) {
                                      texture2D = TextureManager.LoadTextureDXT(data);
                                  }else if (TextureManager.IsPNG(data) || TextureManager.IsJPG(data)) {
                                      texture2D = new Texture2D(2, 2, TextureFormat.DXT5, false);
                                      if (texture2D.LoadImage(data) == false) { texture2D = null; }
                                  } else if (data.IsGIF()) {
                                      UniGif.GifImage gif = UniGif.GetTexturesList(data);
                                      if(gif.frames.Count > 0) {
                                          Logger.LogLine(" frames:" + gif.frames.Count + " loop:" + gif.loopCount + " size:" + gif.width + "x" + gif.height);
                                          gif.Register(__instance.settings.Description.Icon);
                                          UniGif.GifSprites gifSprites = new UniGif.GifSprites(gif);
                                          gifSprites.Register(__instance.settings.Description.Icon);
                                          texture2D = gif.frames.Count > 0 ? gif.frames[0].m_texture2d:null;
                                      }
                                  }
                              }
                              if (texture2D != null) { break; }
                          }
                          if (texture2D != null) { break; }
                        }
                        if(texture2D != null) { break; }
                    }
                    if(texture2D == null) { texture2D = new Texture2D(2, 2); }
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
