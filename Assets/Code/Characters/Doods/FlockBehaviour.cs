using Code.Doods;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class FlockBehaviour : Walk
    {
        public float NeighborhoodRadius = 5f;
        public float Damping = 1f;
        public float AttractWeight = 1f;
        public float RepelWeight = 1f;
        public float AlignWeight = 1f;

        [HideInInspector] public bool IsFlocking = true;

        private Dood _dood;

        protected override void Start () {
            base.Start();
            _dood = GetComponent<Dood>();
        }

        protected override void Move () {
            var dir = WalkingDir;
            if (IsFlocking) {
                var force = CalculateForce() * Time.fixedDeltaTime;
                SetDir(force + dir);
            }

            base.Move();
            if (IsFlocking) {
                SetDir(dir);
            }
        }

        private Vector2 CalculateForce () {
            var center = Vector3.zero;
            var numNearby = 0;
            var force = Vector3.zero;
            var sqrRadius = NeighborhoodRadius * NeighborhoodRadius;
            Vector3 temp;
            foreach (var dood in Game.Ctx.Doods.DoodList) {
                if (dood == _dood) continue;
                var diff = dood.transform.position - transform.position;
                if (!(diff.sqrMagnitude < sqrRadius)) { continue; }

                center += dood.transform.position;
                ++numNearby;
                diff /= diff.sqrMagnitude;
                temp = diff * RepelWeight;
                force -= temp;
                temp = (dood.Character.Velocity - _dood.Character.Velocity) * AlignWeight;
                force += temp;
            }

            if (numNearby == 0) { return Vector3.zero; }

            center /= numNearby;
            temp = (center - transform.position) * AttractWeight;
            force += temp;

            force -= _dood.Character.Velocity * Damping;

            return new Vector2(force.x, force.z);
        }
    }
}