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
    public class CustomPreset {
        public CustomDescription Description = new CustomDescription();
        public bool isCommander;
        public float headMesh = 0.5f;

    }

    public class CustomDescription {
        public string Id;
        public string Name;
        public string Details;
        public string Icon;

    }
}