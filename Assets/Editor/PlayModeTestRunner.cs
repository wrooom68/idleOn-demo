using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Unity.AI.Assistant.PlayModeTest
{
    [InitializeOnLoad]
    internal static class PlayModeTestRunner
    {
        private const string StateKey = "PlayModeTest.State";
        private const string ResultKey = "PlayModeTest.Result";
        private const string ScriptPathKey = "PlayModeTest.ScriptPath";
        private const string SentinelLog = "PLAY_MODE_TEST_COMPLETE";

        private static readonly int WaitFrames = SessionState.GetInt("PlayModeTest.WaitFrames", 15);
        private static readonly float TestTimeout = SessionState.GetFloat("PlayModeTest.TestTimeout", 12.0f);

        private static List<string> _capturedLogs = new List<string>();
        private const int MaxCapturedLogs = 50;

        static PlayModeTestRunner()
        {
            string state = SessionState.GetString(StateKey, "Idle");

            switch (state)
            {
                case "Idle":
                    break;

                case "WaitingForCompile":
                    Debug.Log("[PlayModeTest] Bootstrap compiled. Scheduling Play Mode entry.");
                    EditorApplication.delayCall += () =>
                    {
                        SessionState.SetString(StateKey, "EnteringPlayMode");
                        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
                        EditorApplication.isPlaying = true;
                    };
                    break;

                case "EnteringPlayMode":
                    EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
                    if (EditorApplication.isPlaying)
                    {
                        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                        SessionState.SetString(StateKey, "InPlayMode");
                        EditorApplication.update += WaitFramesThenRun;
                    }
                    break;

                case "InPlayMode":
                    if (EditorApplication.isPlaying)
                    {
                        EditorApplication.update += WaitFramesThenRun;
                    }
                    break;

                case "Done":
                    Debug.Log(SentinelLog);
                    EditorApplication.delayCall += SelfDestruct;
                    break;
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                SessionState.SetString(StateKey, "InPlayMode");
                EditorApplication.update += WaitFramesThenRun;
            }
        }

        private static int _frameCount = 0;
        private static bool _setupDone = false;
        private static bool _testDone = false;
        private static double _testStartTime = 0;

        private static void WaitFramesThenRun()
        {
            _frameCount++;
            if (_frameCount < WaitFrames) return;

            if (_testDone) return;

            if (!_setupDone)
            {
                _setupDone = true;
                Application.logMessageReceived += OnLogMessage;
                _testStartTime = EditorApplication.timeSinceStartup;
                try
                {
                    Setup();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[PlayModeTest] Setup threw exception: " + e);
                    FinishTest(true, e.Message);
                    return;
                }
                return;
            }

            float elapsed = (float)(EditorApplication.timeSinceStartup - _testStartTime);
            bool timedOut = elapsed >= TestTimeout;

            try
            {
                bool complete = Tick(elapsed);
                if (complete || timedOut)
                {
                    FinishTest(timedOut && !complete, timedOut ? "Test timed out after " + TestTimeout + "s" : null);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("[PlayModeTest] Tick threw exception: " + e);
                FinishTest(true, e.Message);
            }
        }

        private static void FinishTest(bool isError, string errorMessage)
        {
            _testDone = true;
            EditorApplication.update -= WaitFramesThenRun;
            Application.logMessageReceived -= OnLogMessage;

            string resultJson;
            try
            {
                resultJson = GetResult();
            }
            catch (System.Exception e)
            {
                resultJson = JsonUtility.ToJson(new TestResult
                {
                    success = false,
                    error = "GetResult() threw: " + e.Message,
                    logs = _capturedLogs.ToArray()
                });
            }

            if (isError && errorMessage != null)
            {
                resultJson = JsonUtility.ToJson(new TestResult
                {
                    success = false,
                    error = errorMessage,
                    logs = _capturedLogs.ToArray()
                });
            }

            SessionState.SetString(ResultKey, resultJson);
            SessionState.SetString(StateKey, "Done");
            EditorApplication.isPlaying = false;
        }

        private static void OnLogMessage(string message, string stackTrace, LogType type)
        {
            if (_capturedLogs.Count >= MaxCapturedLogs) return;
            if (type == LogType.Error || type == LogType.Exception ||
                message.Contains("[Test]") || message.Contains("TEST_RESULT"))
            {
                _capturedLogs.Add("[" + type + "] " + message);
            }
        }

        private static void SelfDestruct()
        {
            string scriptPath = SessionState.GetString(ScriptPathKey, "");
            if (!string.IsNullOrEmpty(scriptPath) && AssetDatabase.AssetPathExists(scriptPath))
            {
                AssetDatabase.DeleteAsset(scriptPath);
            }
            SessionState.EraseString(StateKey);
            SessionState.EraseString(ScriptPathKey);
        }

        [System.Serializable]
        private class TestResult
        {
            public bool success;
            public string error;
            public string[] logs;
            public string details;
        }

        private static void Setup()
        {
            Debug.Log("[Test] Loading CombatZone scene additive in Play Mode.");
            SceneManager.LoadScene("CombatZone", LoadSceneMode.Single);
        }

        private static int _ticksAfterLoad = 0;
        private static bool _playerTriggeredTownPortal = false;
        private static bool _transitionedToTown = false;

        private static bool Tick(float elapsed)
        {
            _ticksAfterLoad++;

            var player = GameObject.Find("Player_World");
            if (player == null)
            {
                return false;
            }

            var portal = GameObject.Find("TownPortal_World");
            if (portal == null)
            {
                // Let's print all gameobjects to find out what actually exists
                var allGos = GameObject.FindObjectsOfType<GameObject>();
                Debug.Log("[Test] Total GameObjects in scene: " + allGos.Length);
                foreach (var go in allGos)
                {
                    if (go.name.Contains("Portal") || go.name.Contains("World") || go.transform.parent == null)
                    {
                        Debug.Log("[Test]   Go: " + go.name + ", active=" + go.activeInHierarchy + ", parent=" + (go.transform.parent != null ? go.transform.parent.name : "null"));
                    }
                }
                Debug.LogError("[Test] TownPortal_World not found in CombatZone!");
                return true;
            }

            // Verify active scene is Town
            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == "Town")
            {
                _transitionedToTown = true;
                Debug.Log("[Test] Verified: Trigger portal successfully transitioned scene back to Town!");
                return true;
            }

            // At tick 5, teleport player directly onto TownPortal_World to simulate triggering it
            if (!_playerTriggeredTownPortal && _ticksAfterLoad >= 5)
            {
                Debug.Log("[Test] Teleporting player onto TownPortal_World to trigger scene load.");
                player.transform.position = portal.transform.position;
                _playerTriggeredTownPortal = true;
            }

            if (elapsed >= 5.0f)
            {
                Debug.Log("[Test] Verification complete.");
                return true;
            }

            return false;
        }

        private static string GetResult()
        {
            bool success = _transitionedToTown;
            string details = $"Transitioned to Town? {_transitionedToTown}";

            Debug.Log("[Test] Final Result - " + details);

            var result = new TestResult
            {
                success = success,
                logs = _capturedLogs.ToArray(),
                details = details
            };
            return JsonUtility.ToJson(result);
        }
    }
}