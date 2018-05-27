using System.Collections.Generic;
using Code.Characters.Doods.LifeCycle;
using Code.Characters.Player.Doodex;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class DoodManager : IContextManager
    {
        public Doodex Doodex { get; private set; }

        private static Object _prefab;

        public readonly List<Dood> DoodList = new List<Dood>();


        public void Initialize () {
            MakeNDoods(1, new Vector3(-10f, 0f, 20f));

            if (Game.Ctx.InMenu) { return; }

            Doodex = new Doodex();
        }

        public void ShutDown () { }

        private void MakeNDoods (int count, Vector3 startpos) {
            const float diff = 1.2f;
            for (var i = 0; i < count; i++) {
                var pos = startpos + new Vector3((i % 5) * diff, 8f, (-i / 5) * diff);
                MakeDood(pos);
            }
        }

        private Dood MakeDood (Vector3 pos, Species species = Species.Debug) {
            var prefab = Resources.Load("Doods/Base Dood");
            var go = (GameObject) Object.Instantiate(prefab, pos, Quaternion.identity);
            var dood = go.GetComponent<Dood>();
            dood.GetComponent<Growth>().SetSpecies(species);

            DoodList.Add(dood);
            
            return dood;
        }


        public void DeselectAll () {
            foreach (var dood in DoodList) {
                dood.OnDeselect();
            }
        }

        public void PurchaseSeed (SeedInfo info, Species species = Species.Debug) {
            var tr = Game.Ctx.Player.transform;
            var pos = tr.position + tr.forward * 3f + Vector3.up * 3f;
            MakeDood(pos);
        }
    }
}