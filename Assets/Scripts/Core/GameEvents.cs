using System;

namespace IdleGuildDemo.Core
{
    /// <summary>
    /// Placeholder event hub for future cross-system notifications.
    /// </summary>
    public static class GameEvents
    {
        public static event Action<string> ToastRequested;
        public static event Action SaveRequested;

        public static void RequestToast(string message)
        {
            ToastRequested?.Invoke(message);
        }

        public static void RequestSave()
        {
            SaveRequested?.Invoke();
        }
    }
}
