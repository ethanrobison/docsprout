using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils
{
    public enum UIPrefab
    {
        Doodex,
        NeedMeter,
        SeedSlot,
    }

    public static class UIUtils
    {
        private static readonly Dictionary<UIPrefab, string> PrefabsPaths = new Dictionary<UIPrefab, string> {
            { UIPrefab.Doodex, "UI/Doodex" },
            { UIPrefab.NeedMeter, "UI/Need" },
            { UIPrefab.SeedSlot, "UI/Seed Slot" }
        };

        public static Transform GetCanvas () {
            return GameObject.Find("Canvas").transform; // hope there's only ever one canvas
        }

        /// <summary>
        /// Makes a GameObject (given a UIPrefab enum) and returns it.
        /// </summary>
        public static GameObject MakeUIPrefab (UIPrefab prefab, Transform holder = null) {
            holder = holder != null ? holder : GetCanvas(); // for some reason ?? doesn't work?!
            string path;
            if (!PrefabsPaths.TryGetValue(prefab, out path)) {
                Logging.Error("Invalid path for prefab: " + prefab);
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
            if (child == null) {
                Logging.Error("Missing child: " + name);
                return null;
            }

            var comp = child.gameObject.GetComponent<T>();
            Logging.Assert(comp != null, "Child missing component of type: " + typeof(T));
            return comp;
        }
    }
}