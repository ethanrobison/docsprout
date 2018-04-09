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

	public float MaxSpeed = 5f;
	public float SpeedUpAcceleration = 30f;
	public float SlowDownAcceleration = 40f;
	public float TurningSpeed = 50f;

	Character _character;
	bool _isSwitchingDirs;

	[HideInInspector] public Vector2 WalkingDir { get; private set; }


	void Start ()
	{
		_character = GetComponent<Character> ();
	}

	void FixedUpdate ()
	{
		Move ();
	}

	void Move ()
	{
		float goalSpeed = GetGoalSpeed ();
		Vector3 goalWalkingDir = GetGoalDir ();

		// Find the component of the character's velocity parallel and perpendicular to the ground
		Vector3 slopedVelocity = _character.velocity;
		float normalVelocity = Vector3.Dot (_character.velocity, _character.groundNormal); // perpendicular to slope
		slopedVelocity -= normalVelocity * _character.groundNormal; // parallel to slope

		// The character's speed in the goal direction
		float parallelSpeed = Vector3.Dot (slopedVelocity, goalWalkingDir);

		// Find the direction and speed perpendicular to the goal direction
		Vector3 perpendicularDir = slopedVelocity - goalWalkingDir * parallelSpeed;
		float perpendicularSpeed = perpendicularDir.magnitude;
		perpendicularDir /= perpendicularSpeed + 0.000001f;

		// Find the change in velocity to use when slowing down now so it doesn't
		// need to be calculated twice if the character is slowing down, since it
		// is also used to cancel out the perpendicular velocity
		float slowingDV = SlowDownAcceleration * Time.fixedDeltaTime;

		// Find out whether or not we are switching directions
		if (!_isSwitchingDirs) _isSwitchingDirs = parallelSpeed / slopedVelocity.magnitude < -0.1f;
		if (_isSwitchingDirs) { // Switching directions
			float dV = TurningSpeed * Time.fixedDeltaTime;

			// Check if we have finished switching directions and also prevent overcorrection
			if (dV > goalSpeed - parallelSpeed) {
				parallelSpeed = goalSpeed;
				_isSwitchingDirs = false;
			} else {
				parallelSpeed += dV;
			}
		} else if (parallelSpeed < goalSpeed) { // Speeding up
			parallelSpeed += SpeedUpAcceleration * Time.fixedDeltaTime;
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
		_character.velocity =
					  parallelSpeed * goalWalkingDir
					+ perpendicularSpeed * perpendicularDir
					+ normalVelocity * _character.groundNormal;

		// Apply gravity
		_character.velocity += Physics.gravity * Time.fixedDeltaTime;
	}


	Vector3 GetGoalDir()
	{
		Vector3 goalWalkingDir = new Vector3 (WalkingDir.x, 0f, WalkingDir.y);
		goalWalkingDir -= Vector3.Dot (goalWalkingDir, _character.groundNormal) * _character.groundNormal;
		goalWalkingDir = goalWalkingDir.normalized;
		return goalWalkingDir;
	}

	float GetGoalSpeed ()
	{
		float walkMagnitude = Mathf.Min (WalkingDir.magnitude, 1);
		float goalSpeed = walkMagnitude * MaxSpeed;
		return goalSpeed;
	}


	// 
	// public-facing stuff

	public void SetDir (Vector2 heading)
	{
		WalkingDir = heading;
	}

	public void SetDir (Vector3 heading)
	{
		WalkingDir = new Vector2 (heading.x, heading.z);
	}
}