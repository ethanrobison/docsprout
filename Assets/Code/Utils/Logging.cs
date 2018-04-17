namespace Code.Utils
{
    public static class Logging
    {
        public static void Log (string message) {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#endif
        }

        public static void Assert (bool condition, string message) {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(condition, message);
#endif
        }

        public static void Warn (string message) {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#endif
        }

        public static void Error (string message) {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#endif
        }
    }
}