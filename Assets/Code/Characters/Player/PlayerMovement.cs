using Code.Characters.Doods;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Player
{
    public class PlayerMovement : Movement
    {
        private const float ACCELERATION = 8f;

        public LayerMask GroundLayer;

        private float _radius;
        private Vector3 _groundNormal;
        private CapsuleCollider _cap;

        protected override void Start () {
            base.Start();
            _cap = gameObject.GetRequiredComponent<CapsuleCollider>();
            _radius = _cap.radius * .9f * transform.lossyScale.x;
        }

        private void SetGroundNormal () {
            var origin = transform.position +
                         Vector3.Scale(((_cap.height / 2f - _cap.radius) * Vector3.down), transform.lossyScale);

            RaycastHit hit;
            var onGround = Physics.SphereCast(origin, _radius, Vector3.down,
                out hit, 0.2f, GroundLayer.value, QueryTriggerInteraction.Ignore);

            if (onGround) {
                _groundNormal = hit.normal;
                Rb.velocity = Rb.velocity - _groundNormal * Vector3.Dot(Rb.velocity, _groundNormal);
            }
            else {
                _groundNormal = Vector3.up;
            }
        }

        protected override void Move () {
            var vel = Rb.velocity;
            var goal = Direction * Speed;

            vel = vel.magnitude * (vel - _groundNormal * Vector3.Dot(vel, _groundNormal)).normalized;
            goal = goal.magnitude * (goal - _groundNormal * Vector3.Dot(goal, _groundNormal)).normalized;

            var error = goal - vel;
            Rb.AddForce(ACCELERATION * error);
        }

        protected override void FixedUpdate () {
            SetGroundNormal();
            base.FixedUpdate();
        }
    }
}