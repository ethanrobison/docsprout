using UnityEngine;

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
							  Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {


			groundNormal = hit.normal;
			isOnGround = true;
			velocity -= Vector3.Dot (velocity, groundNormal) * groundNormal;
		} else {
			isOnGround = false;
			groundNormal = Vector3.up;
		}
	}

	void FixedUpdate ()
	{
		// Move the character and record the collision flags
		collisionFlags = characterController.Move (velocity * Time.fixedDeltaTime);

		// Check whether the character is on the ground
		CheckGrounded ();
	}
}
