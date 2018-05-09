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

    public class Settings {
        public string newCommanderVoice;
    }
}