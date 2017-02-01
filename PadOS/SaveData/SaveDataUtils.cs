using System;
using Newtonsoft.Json;
using System.IO;

namespace PadOS.SaveData {
	public static class SaveDataUtils{
		public const string Directory = @"\Settings\";

		public static void Init(){
			MainPanel.Load();
		}

		public static void SaveData(string filename, object data){
			var directory = Environment.CurrentDirectory + Directory;
			var dataRaw = JsonConvert.SerializeObject(data, Formatting.Indented);
			if (System.IO.Directory.Exists(directory) == false)
				System.IO.Directory.CreateDirectory(directory);
			File.WriteAllText(directory + filename, dataRaw);
		}

		public static T LoadData<T>(string filename) where T:class{
			filename = Environment.CurrentDirectory + Directory + filename;
			if (File.Exists(filename) == false)
				return null;
			var text = File.ReadAllText(filename);
			var data = JsonConvert.DeserializeObject<T>(text);
			return data;
		}
	}
}
