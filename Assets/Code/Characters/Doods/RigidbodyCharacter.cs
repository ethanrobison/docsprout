using UnityEngine;

namespace Code.Characters.Doods
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class RigidbodyCharacter : Character
    {
        Rigidbody _rb;
        CapsuleCollider _capsule;

        // Use this for initialization
        private void Start () {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _capsule = GetComponent<CapsuleCollider>();
        }

        private void CheckGrounded () {
            RaycastHit hit;
            var origin = transform.position +
                         (Vector3.down * (_capsule.height / 2f - _capsule.radius) +
                          transform.rotation * _capsule.center) *
                         transform.lossyScale.z + Vector3.up * .1f;
            if (Physics.SphereCast(origin, _capsule.radius * transform.lossyScale.z * .95f, Vector3.down, out hit, .4f,
                GroundLayers, QueryTriggerInteraction.Ignore)) {
                GroundNormal = hit.normal;
                if (hit.normal.y > .5f) {
                    IsOnGround = true;
                    Velocity.y = 0f;
                    Velocity -= Vector3.Dot(Velocity, GroundNormal) * GroundNormal;
                }
                else {
                    GroundNormal = Vector3.up;
                }
            }
            else {
                IsOnGround = false;
                GroundNormal = Vector3.up;
            }
        }

        private void FixedUpdate () {
            _rb.MovePosition(_rb.position + Velocity * Time.deltaTime);
            CheckGrounded();
            _rb.velocity = Vector3.zero;
        }
    }
}