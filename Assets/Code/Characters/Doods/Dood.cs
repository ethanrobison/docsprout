using Code.Characters.Doods.AI;
using Code.Characters.Doods.Needs;
using Code.Characters.Player.Interaction;
using Code.Environment;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods
{
    [RequireComponent(typeof(FlockBehaviour))]
    public class Dood : MonoBehaviour, ISelectable, IApproachable
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
            Comps.Flock.SetDir(direction);
            return false;
        }

        public void StopMoving () {
            Comps.Flock.IsFlocking = false;
            Comps.Flock.SetDir(Vector2.zero);
        }


        //
        // Selectable interface

        public void OnSelect () {
            IsSelected = true;
            Comps.Color.IsHighlighted = true;
        }

        public void OnDeselect () {
            IsSelected = false;
            Comps.Color.IsHighlighted = false;
        }

        public void OnApproach () { Comps.Status.OnApproach(); }
        public void OnDepart () { Comps.Status.OnDepart(); }
    }

    public class DoodComponents
    {
        public FlockBehaviour Flock { get; private set; }
        public Movement Movement { get; private set; }

        public DoodColor Color { get; private set; }
        public Root Behavior { get; private set; }

        public DoodStatus Status { get; private set; }

        public Animations Animations { get; private set; }


        public DoodComponents (Dood dood) {
            var go = dood.gameObject;
            Flock = go.GetRequiredComponent<FlockBehaviour>();
            Movement = go.GetRequiredComponent<Movement>();

            Color = go.GetRequiredComponentInChildren<DoodColor>();
            Behavior = go.GetRequiredComponentInChildren<BehaviorTree>().Root;

            Status = go.GetRequiredComponentInChildren<DoodStatus>();

            Animations = go.GetRequiredComponent<Animations>();
        }
    }
}