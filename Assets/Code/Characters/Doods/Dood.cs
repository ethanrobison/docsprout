using System.Collections;
using Code.Characters.Doods.AI;
using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods
{
    [RequireComponent(typeof(FlockBehaviour))]
    public class Dood : MonoBehaviour, ISelectable
    {
        [HideInInspector] public bool IsSelected;
        public DoodComponents Comps { get; private set; }


        private void Start () { Comps = new DoodComponents(this); }

        public bool MoveTowards (Vector3 pos, float minDist = 3f) {
            var dist = Vector3.Distance(pos, transform.position);
            if (dist < minDist) {
                StopMoving();
                return true;
            }

            var direction = (pos - transform.position).normalized;
            Comps.Flock.IsFlocking = true;
            Comps.Flock.AlignWeight = .4f;
            Comps.Flock.SetDir(direction);
            return false;
        }

        public void StopMoving () {
            Comps.Flock.IsFlocking = false;
            Comps.Flock.SetDir(Vector2.zero);
        }


        //
        // Selectable interface

        void ISelectable.OnSelect () {
            IsSelected = true;
            Comps.Color.IsHighlighted = true;
        }

        void ISelectable.OnDeselect () {
            IsSelected = false;
            Comps.Color.IsHighlighted = false;
        }
    }

    public class DoodComponents
    {
        public Character Character { get; private set; }
        public FlockBehaviour Flock { get; private set; }

        public DoodColor Color { get; private set; }
        public Root Behavior { get; private set; }
        public Need[] Needs { get; private set; }


        private Transform _behavior;
        private Transform _visuals;
        private Transform _status;

        public DoodComponents (Dood dood) {
            var go = dood.gameObject;
            Flock = go.GetRequiredComponent<FlockBehaviour>();
            Character = go.GetRequiredComponent<Character>();

            Color = go.GetRequiredComponentInChildren<DoodColor>();
            Behavior = go.GetRequiredComponentInChildren<BehaviorTree>().Root;
            Needs = go.GetRequiredComponents<Need>();
        }
    }
}