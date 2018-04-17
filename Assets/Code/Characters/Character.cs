using UnityEngine;

namespace Code.Characters
{
    /// <summary>
    /// Handles basic character needs. Records the velocity and moves the gamobject accordingly
    /// every physics update and stores the collision flags, checks whether or not the gameobject 
    /// is touching the ground and stores the ground normal.
    /// </summary>
    public class Character : MonoBehaviour
    {
        CharacterController _characterController;

        [HideInInspector] public Vector3 Velocity = Vector3.zero;
        [HideInInspector] public bool IsOnGround;
        [HideInInspector] public CollisionFlags CollisionFlags;
        [HideInInspector] public Vector3 GroundNormal = Vector3.up;
        public LayerMask GroundLayers;


        private void Start () {
            _characterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Casts a sphere down to check whether or not the character is touching the ground,
        /// and records the ground's surface normal. If the character is not touching the 
        /// ground, sets the normal to Vector3.up
        /// </summary>
        private void CheckGrounded () {
            RaycastHit hit;
            var origin = transform.position -
                         Vector3.up * (_characterController.height / 2f - _characterController.radius);
            if (Physics.SphereCast(origin, _characterController.radius, Vector3.down, out hit,
                2f * _characterController.skinWidth,
                GroundLayers, QueryTriggerInteraction.Ignore)) {
                GroundNormal = hit.normal;
                if (hit.normal.y > .5f) {
                    IsOnGround = true;
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
            CollisionFlags = _characterController.Move(Velocity * Time.fixedDeltaTime);
            CheckGrounded();
        }
    }
}