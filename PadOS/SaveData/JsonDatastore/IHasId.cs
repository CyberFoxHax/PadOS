
namespace PadOS.SaveData.JsonDatastore
{

    // doesn't work. Actually implementing this interface will cause JsonForeignKeySerializer.Deserialize() to throw an exception
    public interface IHasId {
        System.Int64 Id { get; }
    }
}