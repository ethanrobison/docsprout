using Code.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class Movement : MonoBehaviour
    {
        private const float ACCELERATION = 5f;

        public float Speed = 8f;

        public Vector3 Velocity {
            get { return Rb.velocity; }
        }

        protected Rigidbody Rb;
        protected Vector3 Direction;

        protected virtual void Start () { Rb = gameObject.GetRequiredComponent<Rigidbody>(); }

        protected virtual void FixedUpdate () {
            Move();
            FixRotation(Time.fixedDeltaTime);
        }

        protected virtual void Move () {
            var vel = Rb.velocity;
            var goal = Direction * Speed;
            var error = new Vector3(goal.x - vel.x, 0f, goal.z - vel.z);
            Rb.AddForce(ACCELERATION * error);
        }

        private void FixRotation (float dt) {
            if (Direction.sqrMagnitude < 0.01f) { return; }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), 12f * dt);
        }

        public void SetDirection (Vector2 direction) { SetDirection(new Vector3(direction.x, 0f, direction.y)); }
        public void SetDirection (Vector3 direction) { Direction = direction; }
    }
}