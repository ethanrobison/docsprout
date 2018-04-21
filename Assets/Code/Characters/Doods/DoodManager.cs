using System.Collections.Generic;
using Code.Doods;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class DoodManager : IContextManager
    {
        private static Object _prefab;

        public readonly List<Dood> DoodList = new List<Dood>();

        public void Initialize () { MakeNDoods(50, new Vector3(-10f, 0f, 20f)); }

        public void ShutDown () { }

        private void MakeNDoods (int count, Vector3 startpos) {
            const float diff = 1.2f;
            for (var i = 0; i < count; i++) {
                var pos = startpos + new Vector3((i % 5) * diff, 8f, -i * diff);
                Logging.Log(pos.ToString());
                MakeDood(pos, "Base Dood");
            }
        }

        private void MakeDood (Vector3 pos, string name) {
            var prefab = Resources.Load("Doods/" + name);
            var go = (GameObject) Object.Instantiate(prefab, pos, Quaternion.identity);
            var dood = go.GetComponent<Dood>();

            DoodList.Add(dood);
        }
    }
}