using UnityEngine;

namespace Code.Utils
{
    /// <summary>
    /// Extension methods for GameObjects. Lets you call GetComponent with null checking.
    /// </summary>
    public static class ComponentExtensions
    {
        public static T GetRequiredComponent<T> (this GameObject component) where T : MonoBehaviour {
            var result = component.GetComponent<T>();
            Logging.Assert(result != null, "Missing component of type: " + typeof(T));
            return result;
        }

        public static T[] GetRequiredComponents<T> (this GameObject component) where T : MonoBehaviour {
            var result = component.GetComponents<T>();
            Logging.Assert(result != null && result.Length > 0, "Missing components of type: " + typeof(T));
            return result;
        }

        public static T GetRequiredComponentInChildren<T> (this GameObject component)
            where T : MonoBehaviour {
            var result = component.GetComponentInChildren<T>();
            Logging.Assert(result != null, "Missing components of type: " + typeof(T));
            return result;
        }
    }
}