using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace PadOS.SaveData {
	public static class SaveData{
		public const string Directory = @"\Settings\";

		private static readonly Dictionary<Type, ISaveData<object>> Instances = new Dictionary<Type, ISaveData<object>>();

		public static T Load<T>() where T:class, ISaveData<T>{
			var type = typeof (T);
			var fileName = type.Name + ".json";
			if (Instances.ContainsKey(type))
				return (T) Instances[type];
			return Load<T>(fileName) ?? Save<T>();
		}

		public static T Save<T>() where T:class, ISaveData<object>{
			var type = typeof(T);
			var fileName = type.Name + ".json";

			T realtimeData;
			if (Instances.ContainsKey(type))
				realtimeData = (T) Instances[type];
			else
				realtimeData = (T) Activator.CreateInstance<T>().GetDefault();
			Instances[type] = realtimeData;
			Save(fileName, realtimeData);
			return realtimeData;
		}

		private static void Save(string filename, object data){
			var directory = Environment.CurrentDirectory + Directory;
			var dataRaw = JsonConvert.SerializeObject(data, Formatting.Indented);
			if (System.IO.Directory.Exists(directory) == false)
				System.IO.Directory.CreateDirectory(directory);
			File.WriteAllText(directory + filename, dataRaw);
		}

		private static T Load<T>(string filename) where T:class{
			filename = Environment.CurrentDirectory + Directory + filename;
			if (File.Exists(filename) == false)
				return null;
			var text = File.ReadAllText(filename);
			var data = JsonConvert.DeserializeObject<T>(text);
			return data;
		}
	}
}
