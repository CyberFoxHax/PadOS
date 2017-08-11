using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using PadOS.SaveData.Models;

namespace PadOS.SaveData {
	public class SaveData : DbContext {
		public SaveData()
			: base(new SQLiteConnection($"Data Source={DbFileName};"), true) {
			
		}

		public const string DbFileName = "Settings\\PadOsDatabase.sqlite";

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			var sqliteConnectionInitializer = new SQLite.CodeFirst.SqliteCreateDatabaseIfNotExists<SaveData>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}

		public void DeleteIfExists(){
			if (File.Exists(DbFileName) == false)
				return;
			if (Database.Connection.DataSource == null) 
				Database.Connection.Open();
			var databaseFile = ((SQLiteConnection)Database.Connection).FileName;
			Database.Connection.Close();
			if (File.Exists(databaseFile) == false)
				return;
			Database.Connection.Close();
			File.Delete(databaseFile);
		}

		public void CreateDb(){
			FunctionButtons.ToList();
		}

		public void InsertDefault(){
			FunctionButtons.AddRange(DefaultData.FunctionButtons);
			MainPanelData.AddRange(DefaultData.MainPanelData);
			SaveChanges();
		}

		public virtual DbSet<FunctionButton> FunctionButtons { get; set; }
		public virtual DbSet<MainPanelData> MainPanelData { get; set; }
	}
}
