using System;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Domain event channel for quest-relevant gameplay outcomes.
    /// </summary>
    public static class QuestGameplayEvents
    {
        public static event Action<string> EnemyKilled;
        public static event Action<string, int> ItemCollected;
        public static event Action<string, int> ItemCrafted;
        public static event Action<int> LevelReached;
        public static event Action<string> ClassChosen;
        public static event Action<string> CharacterUnlocked;

        public static void RaiseEnemyKilled(string enemyId)
        {
            EnemyKilled?.Invoke(enemyId ?? string.Empty);
        }

        public static void RaiseItemCollected(string itemId, int amount)
        {
            ItemCollected?.Invoke(itemId ?? string.Empty, amount);
        }

        public static void RaiseItemCrafted(string itemId, int amount)
        {
            ItemCrafted?.Invoke(itemId ?? string.Empty, amount);
        }

        public static void RaiseLevelReached(int level)
        {
            LevelReached?.Invoke(level);
        }

        public static void RaiseClassChosen(string classId)
        {
            ClassChosen?.Invoke(classId ?? string.Empty);
        }

        public static void RaiseCharacterUnlocked(string characterId)
        {
            CharacterUnlocked?.Invoke(characterId ?? string.Empty);
        }
    }
}
