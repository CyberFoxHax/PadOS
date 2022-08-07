namespace PadOS.ProfileExecution {
    public interface IActionHandler {
        void Init(SaveData.ProfileXML.IAction actionNode);
        bool Enabled { get; set; }
        void Invoke();
    }
}
