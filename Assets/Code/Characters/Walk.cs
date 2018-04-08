using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component used for handling walking. To use, set walkingDir in the direction you want
/// the character to walk in with the magnitude being the portion of the component's
/// maxSpeed you want the character's speed to be. The component accelerates the character
/// using three separate accelerations, one for speeding up, one for slowing down, and one
/// for switching directions. The component will move the character parallel to the ground.
/// </summary>
[RequireComponent (typeof (Character))]
public class Walk : MonoBehaviour {

	public float maxSpeed = 5f;
	public float speedUpAcceleration = 30f;
	public float slowDownAcceleration = 40f;
	public float switchDirectionsAcceleration = 50f;

	Character character;
	bool isSwitchingDirs;

	[HideInInspector] public Vector2 walkingDir;


	void Start ()
	{
		character = GetComponent<Character> ();
	}

	void FixedUpdate ()
	{
		// Calculate how fast the character should aim to be moving
		float walkMagnitude = Mathf.Min (walkingDir.magnitude, 1);
		float goalSpeed = walkMagnitude * maxSpeed;

		// Find the goal velocity, ensuring it is parallel to the ground
		Vector3 goalWalkingDir = new Vector3 (walkingDir.x, 0f, walkingDir.y);
		goalWalkingDir -= Vector3.Dot (goalWalkingDir, character.groundNormal) * character.groundNormal;
		goalWalkingDir = goalWalkingDir.normalized;

		// Find the component of the character's velocity parallel and perpendicular to the ground
		Vector3 slopedVelocity = character.velocity;
		float normalVelocity = Vector3.Dot (character.velocity, character.groundNormal); // perpendicular to slope
		slopedVelocity -= normalVelocity * character.groundNormal; // parallel to slope

		// The character's speed in the goal direction
		float parallelSpeed = Vector3.Dot (slopedVelocity, goalWalkingDir);

		// Find the direction and speed perpendicular to the goal direction
		Vector3 perpendicularDir = slopedVelocity - goalWalkingDir * parallelSpeed;
		float perpendicularSpeed = perpendicularDir.magnitude;
		perpendicularDir /= perpendicularSpeed + 0.000001f;

		// Find the change in velocity to use when slowing down now so it doesn't
		// need to be calculated twice if the character is slowing down, since it
		// is also used to cancel out the perpendicular velocity
		float slowingDV = slowDownAcceleration * Time.fixedDeltaTime;

		// Find out whether or not we are switching directions
		if (!isSwitchingDirs) isSwitchingDirs = parallelSpeed / slopedVelocity.magnitude < -0.1f;
		if (isSwitchingDirs) { // Switching directions
			float dV = switchDirectionsAcceleration * Time.fixedDeltaTime;

			// Check if we have finished switching directions and also prevent overcorrection
			if (dV > goalSpeed - parallelSpeed) {
				parallelSpeed = goalSpeed;
				isSwitchingDirs = false;
			} else {
				parallelSpeed += dV;
			}
		} else if (parallelSpeed < goalSpeed) { // Speeding up
			parallelSpeed += speedUpAcceleration * Time.fixedDeltaTime;
			parallelSpeed = Mathf.Min (parallelSpeed, goalSpeed); // prevent overcorrection
		} else { // Slowing down
			if (slowingDV > goalSpeed - parallelSpeed) { // prevent overcorrection
				parallelSpeed = goalSpeed;
			} else {
				parallelSpeed -= slowingDV;
			}
		}

		// Velocity perpendicular to the goal direction is always slowing down
		float perpSign = Mathf.Sign (perpendicularSpeed);

		if (slowingDV > perpendicularSpeed * perpSign) {
			perpendicularSpeed = 0;
		} else {
			perpendicularSpeed -= slowingDV * perpSign;
		}

		// Combine all components of velocity
		character.velocity = parallelSpeed * goalWalkingDir + perpendicularSpeed * perpendicularDir + normalVelocity * character.groundNormal;


		// Apply gravity
		character.velocity += Physics.gravity * Time.fixedDeltaTime;

	}
}


