using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class Movement : MonoBehaviour
    {
        public float Speed = 8f;

        public Vector3 Velocity {
            get { return _rb.velocity; }
        }

        private Rigidbody _rb;
        private Vector3 _direction;
        private const float ACCELERATION = 5f;

        private void Start () { _rb = gameObject.GetRequiredComponent<Rigidbody>(); }

        private void FixedUpdate () {
            var vel = _rb.velocity;
            var goal = _direction * Speed;
            var error = new Vector3(goal.x - vel.x, 0f, goal.z - vel.z);
            _rb.AddForce(ACCELERATION * error);
        }

        public void SetDirection (Vector2 direction) { SetDirection(new Vector3(direction.x, 0f, direction.y)); }
        public void SetDirection (Vector3 direction) { _direction = direction.normalized; }
    }
}