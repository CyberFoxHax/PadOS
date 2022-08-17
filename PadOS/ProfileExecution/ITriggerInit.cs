namespace PadOS.ProfileExecution {
    public interface ITriggerInit {
        void Init(SaveData.ProfileXML.ITrigger node, Input.GamePadInput.GamePadInput input);
    }
}
