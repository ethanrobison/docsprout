using System.Collections;
using System.Collections.Generic;
using Code.Characters.Doods;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public abstract class Satisfier : MonoBehaviour
    {
        private void OnTriggerEnter (Collider other) {
            var satisfiable = other.GetComponentInChildren<ISatisfiable>();
            if (satisfiable != null) { satisfiable.AllowSatisfaction(this); }
        }

        private void OnTriggerExit (Collider other) {
            var satisfiable = other.GetComponentInChildren<ISatisfiable>();
            if (satisfiable != null) { satisfiable.ForbidSatisfaction(this); }
        }

        public abstract void InteractWith (Dood user);

        public abstract NeedType Satisfies ();
    }

    public interface ISatisfiable
    {
        void AllowSatisfaction (Satisfier satisfier);
        void ForbidSatisfaction (Satisfier satisfier);
    }
}