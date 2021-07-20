using PadOS.SaveData.JsonDatastore;
using System.IO;
using PadOS.SaveData.Models;

namespace PadOS.SaveData {
    public class SaveData : JsonContext {
        public SaveData() : base("Settings") {

        }

        public bool DatabaseExists() {
            return Directory.Exists(DirectoryName);
        }

        public void DeleteIfExists() {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName);
        }

        public void CreateDb() {
            if (Directory.Exists(DirectoryName) == false)
                Directory.CreateDirectory(DirectoryName);
        }

        public void InsertDefault() {
            DefaultData.InsertData(this);
        }

        public virtual JsonTable<Profile> Profiles { get; set; }
        public virtual JsonTable<Function> Functions { get; set; }
        public virtual JsonTable<PanelButton> PanelButtons { get; set; }
        public virtual JsonTable<ProfileAssociation> ProfileAssociations { get; set; }
    }
}
