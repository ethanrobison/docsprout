using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    public enum UIPrefab
    {
        MainMenu,
    }

    public static class UIUtils
    {
        private static readonly Dictionary<UIPrefab, string> PrefabsPaths = new Dictionary<UIPrefab, string> {
            {UIPrefab.MainMenu, "UI/Main Menu"}
        };

        private static Transform GetCanvas () {
            return GameObject.Find("Canvas").transform; // hope there's only ever one canvas
        }

        /// <summary>
        /// Makes a GameObject (given a UIPrefab enum) and returns it.
        /// </summary>
        public static GameObject MakeUIPrefab (UIPrefab prefab, Transform holder = null) {
            holder = holder ?? GetCanvas();
            string path;
            if (!PrefabsPaths.TryGetValue(prefab, out path)) {
                Debug.LogError("Invalid path for prefab: " + prefab);
                return null;
            }

            var resource = Resources.Load(path);
            return (GameObject) Object.Instantiate(resource, holder);
        }

        /// <summary>
        /// Given a transform and the name of a child, get its component of type T.
        /// Complains if child/component nonexistent.
        /// </summary>
        public static T FindUICompOfType<T> (Transform parent, string name) where T : MonoBehaviour {
            var child = parent.Find(name);
            Debug.Assert(child != null, "Missing child: " + name);

            var comp = child.gameObject.GetComponent<T>();
            Debug.Assert(comp != null, "Child missing component of type: " + typeof(T));

            return comp;
        }
    }
}