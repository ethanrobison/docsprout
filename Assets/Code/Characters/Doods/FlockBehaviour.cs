using UnityEngine;

namespace Code.Characters.Doods
{
    public class FlockBehaviour : MonoBehaviour
    {
        public float NeighborhoodRadius = 5f;
        public float Damping = 1f;
        public float AttractWeight = 1f;
        public float RepelWeight = 1f;
        public float AlignWeight = 1f;

        [HideInInspector] public bool IsFlocking;

        private Vector2 _walkingDir;
        private Dood _dood;


        private void Start () { _dood = GetComponent<Dood>(); }

        private void FixedUpdate () { Move(); }

        private void Move () {
            var result = _walkingDir.magnitude < 0.01f // todo should I be a constant or don't I care
                ? Vector2.zero
                : _walkingDir + (IsFlocking ? CalculateForce() : Vector2.zero);
            _dood.Comps.Movement.SetDirection(result);
        }

        private const float MAX_MAGNITUDE = 0.5f;

        private Vector2 CalculateForce () {
            Vector3 center = Vector3.zero, force = Vector3.zero;
            var numNearby = 0;
            var sqrRadius = NeighborhoodRadius * NeighborhoodRadius;

            foreach (var dood in Game.Ctx.Doods.DoodList) {
                if (dood == _dood) continue;
                var diff = dood.transform.position - transform.position;
                if (!(diff.sqrMagnitude < sqrRadius)) { continue; }

                center += dood.transform.position;
                ++numNearby;
                diff /= diff.sqrMagnitude;
                force -= diff * RepelWeight;
                force += (dood.Comps.Character.Velocity - _dood.Comps.Character.Velocity) * AlignWeight;
            }

            if (numNearby == 0) { return Vector3.zero; }

            center /= numNearby;
            force += (center - transform.position) * AttractWeight;
            force -= _dood.Comps.Character.Velocity * Damping;

            return Vector2.ClampMagnitude(new Vector2(force.x, force.z), MAX_MAGNITUDE);
        }

        // 
        // public-facing stuff

        public void SetDir (Vector2 heading) { _walkingDir = heading; }

        public void SetDir (Vector3 heading) { _walkingDir = new Vector2(heading.x, heading.z); }
    }
}