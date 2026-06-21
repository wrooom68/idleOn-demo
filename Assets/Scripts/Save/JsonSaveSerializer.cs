using System;
using IdleGuildDemo.Runtime;
using UnityEngine;

namespace IdleGuildDemo.Save
{
    /// <summary>
    /// Converts save data to and from Unity JsonUtility JSON.
    /// </summary>
    public sealed class JsonSaveSerializer
    {
        public string ToJson(SaveData data, bool prettyPrint = true)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.Normalize();
            return JsonUtility.ToJson(data, prettyPrint);
        }

        /// <summary>
        /// Returns null when JSON is empty or invalid so callers can decide how to recover.
        /// </summary>
        public SaveData FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                var data = JsonUtility.FromJson<SaveData>(json);
                if (data != null)
                {
                    data.Normalize();
                }

                return data;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to deserialize save data: {exception.Message}");
                return null;
            }
        }
    }
}
