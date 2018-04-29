using System;
using Code.Characters.Doods;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public class Satisfier : MonoBehaviour
    {
        public Action<Dood> OnInteract;
        [SerializeField] private NeedType _type;
        private void OnTriggerEnter (Collider other) {
            var satisfiable = other.GetComponentInChildren<ISatisfiable>();
            if (satisfiable != null) { satisfiable.AllowSatisfaction(this); }
        }

        private void OnTriggerExit (Collider other) {
            var satisfiable = other.GetComponentInChildren<ISatisfiable>();
            if (satisfiable != null) { satisfiable.ForbidSatisfaction(this); }
        }

        public void InteractWith (Dood dood) {
            dood.Comps.Status.GetNeedOfType(Satisfies()).Satisfy();
            OnInteract(dood);
        }

        public NeedType Satisfies () {return _type;}
    }

    public interface ISatisfiable
    {
        void AllowSatisfaction (Satisfier satisfier);
        void ForbidSatisfaction (Satisfier satisfier);
    }
}