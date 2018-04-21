using System.Collections.Generic;
using Code.Doods;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class DoodManager : IContextManager
    {
        private static Object _prefab;

        public readonly List<Dood> DoodList = new List<Dood>();

        public void Initialize () {
            MakeNDoods(5);
        }

        public void ShutDown () { }

        private void MakeNDoods (int n, Vector3 startpos = new Vector3()) {
            for (int i = 0; i < n; i++) {
                var pos = startpos + new Vector3(n * 1.2f, 5f, (n % 5) * 1.2f);
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