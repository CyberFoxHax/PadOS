namespace PadOS.SaveData{
	public interface ISaveData<out T>{
		T GetDefault();
	}
}