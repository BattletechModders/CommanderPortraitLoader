using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using BattleTech;

namespace CommanderPortraitLoader {
  class CustomVoiceFetcher {

    /*public static List<CustomVoice> GetCustomVoices()
    {
        List<CustomVoice> customvoices = new List<CustomVoice>();
        Assembly assembly = getCustomVoiceAssembly();
        if(assembly == null)
        {
            Logger.LogLine("Custom Voices Assembly not found");
            return new List<CustomVoice>();
        }
        try
        {
            Type core = assembly.GetType("CustomVoices.Core");
            FieldInfo voicePacks = core.GetField("extVoicePacks", BindingFlags.Public | BindingFlags.Static);
            var voiceMap = (IDictionary)voicePacks.GetValue(null);
            foreach(DictionaryEntry entry in voiceMap)
            {
                CustomVoice cv = new CustomVoice();
                cv.name = (string)entry.Key;
                Type voiceDef = entry.Value.GetType();
                cv.gender = (Gender)voiceDef.GetProperty("gender", BindingFlags.Public | BindingFlags.Instance).GetValue(entry.Value);
                customvoices.Add(cv);
                Logger.LogLine(string.Format("Found Custom Voice: {0}", entry.Key));

            }

            return customvoices;
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }

        return new List<CustomVoice>();
    }*/
    public static bool isCustomVoicesDetected { get; private set; }

    public static void DetectCustomVoices(List<string> loadOrder) {
      isCustomVoicesDetected = false;
      foreach (string mod in loadOrder) {
        if (mod == "CustomVoices") { isCustomVoicesDetected = true; break; }
      }
    }
    private static Assembly getCustomVoiceAssembly() {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

      foreach (Assembly assembly in assemblies) {
        if (assembly.GetName().Name == "CustomVoices") {
          return assembly;
        }
      }

      return null;


    }


  }
}
