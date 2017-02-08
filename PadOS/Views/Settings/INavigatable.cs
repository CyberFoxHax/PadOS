namespace PadOS.Views.Settings{
	public interface INavigatable {
		bool IsActive { get; set; }
		void OnClick();
		System.Windows.Input.ICommand Click { get; set; }
	}
}