using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Walk : MonoBehaviour {

    Character character;

    [System.Serializable]
    public struct WalkSettings {
        public float maxSpeed;
        public float speedUpAcceleration;
        public float slowDownAcceleration;
        public float switchDirectionsAcceleration;

    }
    public WalkSettings settings;

    bool isSwitchingDirs;

    [HideInInspector] public Vector3 walkingDir;


	void Start () {
        character = GetComponent<Character>();
	}
	


	void FixedUpdate()
	{
        if(character.isOnGround) {
            // Calculate how fast the character should aim to be moving
            float walkMagnitude = Mathf.Min(walkingDir.magnitude, 1);
            float goalSpeed = walkMagnitude*settings.maxSpeed;

            // Find the goal velocity, ensuring it is perpendicular to the ground's normal
            Vector3 goalWalkingDir = walkingDir;
            goalWalkingDir -= Vector3.Dot(goalWalkingDir, character.groundNormal)*character.groundNormal;
            goalWalkingDir = goalWalkingDir.normalized;

            //Find the component of the character's velocity perpendicular to the ground's normal
            Vector3 slopedVelocity = character.velocity;
            float normalVelocity = Vector3.Dot(character.velocity, character.groundNormal);
            slopedVelocity -= normalVelocity*character.groundNormal;

            // Find out whether or not we are switching directions
            float parallelSpeed = Vector3.Dot(slopedVelocity, goalWalkingDir);
            if(!isSwitchingDirs) isSwitchingDirs = parallelSpeed/slopedVelocity.magnitude < -0.1f;

            Vector3 perpendicularDir = slopedVelocity - goalWalkingDir*parallelSpeed;
            float perpendicularSpeed = perpendicularDir.magnitude;
            perpendicularDir.Normalize();

            float slowingDV = settings.slowDownAcceleration*Time.fixedDeltaTime;

            if(isSwitchingDirs) { // Switching directions
                float dV = settings.switchDirectionsAcceleration*Time.fixedDeltaTime;
                if(dV > goalSpeed - parallelSpeed) {
                    parallelSpeed = goalSpeed;
                    isSwitchingDirs = false;
                }
                else {
                    parallelSpeed += dV;
                }
            }
            else if(parallelSpeed < goalSpeed) { // Speeding up
                parallelSpeed += settings.speedUpAcceleration*Time.fixedDeltaTime;
                parallelSpeed = Mathf.Min(parallelSpeed, goalSpeed);
            }
            else { // Slowing down
                if(slowingDV > goalSpeed - parallelSpeed) {
                    parallelSpeed = goalSpeed;
                }
                else {
                    parallelSpeed -= slowingDV;
                }

            }

            // Velocity perpendicular to the goal direction is always slowing down
            float perpSign = Mathf.Sign(perpendicularSpeed);

            if(slowingDV > perpendicularSpeed*perpSign) {
                perpendicularSpeed = 0;
            }
            else {
                perpendicularSpeed -= slowingDV*perpSign;
            }

            character.velocity = parallelSpeed*goalWalkingDir + perpendicularSpeed*perpendicularDir + normalVelocity*character.groundNormal;

        }

        character.velocity += Physics.gravity*Time.fixedDeltaTime;

	}
}


