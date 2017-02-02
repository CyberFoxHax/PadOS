namespace PadOS.Views.Settings{
	public interface INavigatable {
		bool IsActive { get; set; }
		void Activate();
	}
}