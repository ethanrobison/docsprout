using UnityEngine;

namespace Code.Characters {
	/// <summary>
	/// Handles basic character needs. Records the velocity and moves the gamobject accordingly
	/// every physics update and stores the collision flags, checks whether or not the gameobject 
	/// is touching the ground and stores the ground normal.
	/// </summary>
	[RequireComponent (typeof (CharacterController))]
	public class Character : MonoBehaviour {

		CharacterController characterController;

		[HideInInspector] public Vector3 velocity = Vector3.zero;
		[HideInInspector] public bool isOnGround;
		[HideInInspector] public CollisionFlags collisionFlags;
		[HideInInspector] public Vector3 groundNormal = Vector3.up;
		public LayerMask GroundLayers;


		void Start ()
		{
			characterController = GetComponent<CharacterController> ();
		}

		/// <summary>
		/// Casts a sphere down to check whether or not the character is touching the ground,
		/// and records the ground's surface normal. If the character is not touching the 
		/// ground, sets the normal to Vector3.up
		/// </summary>
		void CheckGrounded ()
		{
			RaycastHit hit;
			Vector3 origin = transform.position - Vector3.up * (characterController.height / 2f - characterController.radius);
			if (Physics.SphereCast (origin, characterController.radius, Vector3.down, out hit, 2f * characterController.skinWidth,
								GroundLayers, QueryTriggerInteraction.Ignore)) {


				groundNormal = hit.normal;
				if(hit.normal.y > .5f) {
					isOnGround = true;
					velocity -= Vector3.Dot (velocity, groundNormal) * groundNormal;
				}
				else {
					groundNormal = Vector3.up;
				}
			} else {
				isOnGround = false;
				groundNormal = Vector3.up;
			}
		}

		void FixedUpdate ()
		//void Update()
		{
			// Move the character and record the collision flags
			collisionFlags = characterController.Move (velocity * Time.fixedDeltaTime);
			//collisionFlags = characterController.Move (velocity * Time.deltaTime);

			// Check whether the character is on the ground
			CheckGrounded ();
		}
	}
}

