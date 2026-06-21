using System.IO;
using UnityEngine;

namespace IdleGuildDemo.Save
{
    /// <summary>
    /// Shared save-file names and path helpers.
    /// </summary>
    public static class SaveFilePaths
    {
        public const string SaveFileName = "idle_guild_save.json";

        public static string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }
    }
}
