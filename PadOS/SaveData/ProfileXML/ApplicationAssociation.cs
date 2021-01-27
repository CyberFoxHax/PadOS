namespace PadOS.SaveData.ProfileXML
{
    public class ApplicationAssociation {
        public string Executable { get; set; }
        public string WindowTitle { get; set; }
        public Profile Profile { get; set; }
        public string DllName { get; internal set; }
    }
}