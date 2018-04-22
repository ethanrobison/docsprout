using UnityEngine;
using Code.Utils;

namespace Code.Characters.Player
{
    public class PlayerMovement : Doods.Movement
    {
        private const float ACCELERATION = 8f;

        public LayerMask GroundLayer;

        private Vector3 _groundNormal;
        private CapsuleCollider _cap;

        protected override void Start () {
            base.Start();
            _cap = gameObject.GetRequiredComponent<CapsuleCollider>();
        }

        private void SetGroundNormal () {
            var origin = transform.position +
                             Vector3.Scale(((_cap.height/2f - _cap.radius) * Vector3.down), transform.lossyScale);

            RaycastHit hit;
            bool onGround = Physics.SphereCast(origin, _cap.radius * .9f * transform.lossyScale.x, Vector3.down,
                out hit, 0.2f, GroundLayer.value, QueryTriggerInteraction.Ignore);
            
            Logging.Log(onGround.ToString());
            if (onGround) {
                _groundNormal = hit.normal;
                _rb.velocity = _rb.velocity - _groundNormal*Vector3.Dot(_rb.velocity, _groundNormal);
            }
            else {
                _groundNormal = Vector3.up;
            }
        }
        
        protected override void Move () {
            var vel = _rb.velocity;
            var goal = _direction * Speed;
            
            vel = vel.magnitude*(vel - _groundNormal*Vector3.Dot(vel, _groundNormal)).normalized;
            goal = goal.magnitude*(goal - _groundNormal*Vector3.Dot(goal, _groundNormal)).normalized;

            
            var error = goal - vel;
            _rb.AddForce(ACCELERATION * error);
        }

        protected override void FixedUpdate () {
            SetGroundNormal();
            base.FixedUpdate();
        }
    }
}