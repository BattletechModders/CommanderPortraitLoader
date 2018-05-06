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
using BattleTech.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CommanderPortraitLoader {
    public class Logger {
        public static void LogError(Exception ex) {
            string filePath = "mods/CommanderPortraitLoader/Log.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true)) {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static void LogLine(String line) {
            string filePath = "mods/CommanderPortraitLoader/Log.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true)) {
                writer.WriteLine(line + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}