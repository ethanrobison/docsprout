﻿using Code.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace Code.Characters.Doods
{
    public class Movement : MonoBehaviour
    {
        private const float ACCELERATION = 5f;

        public float Speed = 8f;

        public Vector3 Velocity {
            get { return _rb.velocity; }
        }

        private Rigidbody _rb;
        private Vector3 _direction;

        private void Start () { _rb = gameObject.GetRequiredComponent<Rigidbody>(); }

        private void FixedUpdate () {
            Move();
            FixRotation();
        }

        private void Move () {
            var vel = _rb.velocity;
            var goal = _direction * Speed;
            var error = new Vector3(goal.x - vel.x, 0f, goal.z - vel.z);
            _rb.AddForce(ACCELERATION * error);
        }

        private void FixRotation () {
            if (_direction.sqrMagnitude < 0.01f) { return; }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), 0.1f);
        }

        public void SetDirection (Vector2 direction) { SetDirection(new Vector3(direction.x, 0f, direction.y)); }
        public void SetDirection (Vector3 direction) { _direction = direction; }
    }
}