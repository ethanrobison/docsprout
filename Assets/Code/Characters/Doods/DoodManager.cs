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
            // todo what is going on here
            for (var i = 0; i < 1; i++) {
                for (float y = 0; y < 1.2f; y += 1.2f) {
                    MakeDood(new Vector3(i, 5f, y), "Base Dood");
                }
            }
        }

        public void ShutDown () { }


        private void MakeDood (Vector3 pos, string name) {
            var prefab = Resources.Load("Doods/" + name);
            var go = (GameObject) Object.Instantiate(prefab, pos, Quaternion.identity);
            var dood = go.GetComponent<Dood>();

            DoodList.Add(dood);
        }
    }
}