using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class SaveData
    {
        public int saveVersion = 1;
        public PlayerProfile profile = new PlayerProfile();
        public string createdUtc = string.Empty;
        public string lastSavedUtc = string.Empty;

        public static SaveData CreateNew()
        {
            var now = DateTime.UtcNow.ToString("o");
            return new SaveData
            {
                saveVersion = 1,
                profile = PlayerProfile.CreateDefault(),
                createdUtc = now,
                lastSavedUtc = now
            };
        }

        public void Normalize()
        {
            if (saveVersion <= 0)
            {
                saveVersion = 1;
            }

            if (profile == null)
            {
                profile = PlayerProfile.CreateDefault();
            }
            else
            {
                profile.Normalize();
            }

            if (string.IsNullOrEmpty(createdUtc))
            {
                createdUtc = DateTime.UtcNow.ToString("o");
            }

            if (string.IsNullOrEmpty(lastSavedUtc))
            {
                lastSavedUtc = createdUtc;
            }
        }
    }
}
