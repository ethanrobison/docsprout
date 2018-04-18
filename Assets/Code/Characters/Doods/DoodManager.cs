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
            string[] names = { "Sphere", "Capsule", "Cone", "Cube", "Cylinder" };
            for (var i = 0; i < 5; i++) {
                for (float y = 0; y < 12f; y += 1.2f) {
                    MakeDood(new Vector3(i, 5f, y), names[i]);
                }
            }
        }

        public void ShutDown () { }


        private void MakeDood (Vector3 pos, string name) {
            var prefab = Resources.Load("Doods/" + name + " Dood");
            var go = (GameObject) Object.Instantiate(prefab, pos, Quaternion.Euler(-90f, 0f, 0f));
            var dood = go.GetComponent<Dood>();

            DoodList.Add(dood);
        }
    }
}