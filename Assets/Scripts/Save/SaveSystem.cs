using System;
using System.IO;
using IdleGuildDemo.Runtime;
using UnityEngine;

namespace IdleGuildDemo.Save
{
    /// <summary>
    /// Coordinates JSON save/load for the local save file.
    /// </summary>
    public sealed class SaveSystem
    {
        private readonly JsonSaveSerializer serializer;

        public SaveData CurrentSave { get; private set; }

        public SaveSystem()
            : this(new JsonSaveSerializer())
        {
        }

        public SaveSystem(JsonSaveSerializer serializer)
        {
            this.serializer = serializer ?? new JsonSaveSerializer();
        }

        public SaveData LoadOrCreate()
        {
            var path = GetSavePath();
            if (!File.Exists(path))
            {
                CurrentSave = SaveData.CreateNew();
                Debug.Log($"No save found. Created new save at {path}");
                return CurrentSave;
            }

            var json = File.ReadAllText(path);
            var loaded = serializer.FromJson(json);
            if (loaded == null)
            {
                CurrentSave = SaveData.CreateNew();
                Debug.LogWarning($"Save file at {path} could not be loaded. Created a new save.");
                return CurrentSave;
            }

            CurrentSave = loaded;
            Debug.Log($"Loaded save from {path}");
            return CurrentSave;
        }

        public void Save()
        {
            Save(CurrentSave ?? SaveData.CreateNew());
        }

        public void Save(SaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.Normalize();
            data.lastSavedUtc = DateTime.UtcNow.ToString("o");

            var path = GetSavePath();
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, serializer.ToJson(data, true));
            CurrentSave = data;
            Debug.Log($"Saved game to {path}");
        }

        public bool SaveExists()
        {
            return File.Exists(GetSavePath());
        }

        public void DeleteSave()
        {
            var path = GetSavePath();
            if (!File.Exists(path))
            {
                Debug.Log($"No save file to delete at {path}");
                return;
            }

            File.Delete(path);
            if (CurrentSave != null)
            {
                CurrentSave = null;
            }

            Debug.Log($"Deleted save at {path}");
        }

        public string GetSavePath()
        {
            return SaveFilePaths.GetSavePath();
        }
    }
}
