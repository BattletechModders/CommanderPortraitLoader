namespace CommanderPortraitLoader {
    public class CustomPreset {
        public CustomDescription Description = new CustomDescription();
        public bool isCommander;
        public float headMesh = 0.5f;
//        public string newVoice;
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

    public static class NewVoice
    {
        public static string newVoice;
    }

}