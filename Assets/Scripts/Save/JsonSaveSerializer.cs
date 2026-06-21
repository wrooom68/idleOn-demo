using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Save
{
    /// <summary>
    /// Placeholder for future JSON serialization and deserialization.
    /// </summary>
    public sealed class JsonSaveSerializer
    {
        public string ToJson(SaveData saveData)
        {
            // TODO: Serialize with Unity JsonUtility or another approved serializer.
            return string.Empty;
        }

        public SaveData FromJson(string json)
        {
            // TODO: Deserialize JSON into SaveData after save schema is finalized.
            return new SaveData();
        }
    }
}
