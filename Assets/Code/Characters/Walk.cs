using UnityEngine;

namespace Code.Characters
{
    /// <summary>
    /// Component used for handling walking. To use, set walkingDir in the direction you want
    /// the character to walk in with the magnitude being the portion of the component's
    /// maxSpeed you want the character's speed to be. The component accelerates the character
    /// using three separate accelerations, one for speeding up, one for slowing down, and one
    /// for switching directions. The component will move the character parallel to the ground.
    /// </summary>
    [RequireComponent(typeof(Character))]
    public class Walk : MonoBehaviour
    {
        public float MaxSpeed = 5f;
        public float SpeedUpAcceleration = 30f;
        public float SlowDownAcceleration = 40f;
        public float TurningSpeed = 50f;
        public float AngularSpeed = 12f;

        Character _character;
        bool _isSwitchingDirs;

        protected Vector2 WalkingDir { get; private set; }


        protected virtual void Start () { _character = GetComponent<Character>(); }

        private void Update () {
            if (WalkingDir.sqrMagnitude < 0.01f) return;
            Vector3 goalDir = new Vector3(WalkingDir.x, 0f, WalkingDir.y);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goalDir),
                AngularSpeed * Time.deltaTime);
        }

        protected void FixedUpdate () { Move(); }

        protected virtual void Move () {
            var goalSpeed = GetGoalSpeed();
            var goalWalkingDir = GetGoalDir();

            // Find the component of the character's velocity parallel and perpendicular to the ground
            var slopedVelocity = _character.Velocity;
            var normalVelocity = Vector3.Dot(_character.Velocity, _character.GroundNormal); // perpendicular to slope
            slopedVelocity -= normalVelocity * _character.GroundNormal; // parallel to slope

            // The character's speed in the goal direction
            var parallelSpeed = Vector3.Dot(slopedVelocity, goalWalkingDir);

            // Find the direction and speed perpendicular to the goal direction
            var perpendicularDir = slopedVelocity - goalWalkingDir * parallelSpeed;
            var perpendicularSpeed = perpendicularDir.magnitude;
            perpendicularDir /= perpendicularSpeed + 0.000001f;

            // Find the change in velocity to use when slowing down now so it doesn't
            // need to be calculated twice if the character is slowing down, since it
            // is also used to cancel out the perpendicular velocity
            var slowingDv = SlowDownAcceleration * Time.fixedDeltaTime;

            // Find out whether or not we are switching directions
            if (!_isSwitchingDirs) _isSwitchingDirs = parallelSpeed / slopedVelocity.magnitude < -0.1f;
            if (_isSwitchingDirs) { // Switching directions
                var dV = TurningSpeed * Time.fixedDeltaTime;

                // Check if we have finished switching directions and also prevent overcorrection
                if (dV > goalSpeed - parallelSpeed) {
                    parallelSpeed = goalSpeed;
                    _isSwitchingDirs = false;
                }
                else {
                    parallelSpeed += dV;
                }
            }
            else if (parallelSpeed < goalSpeed) { // Speeding up
                parallelSpeed += SpeedUpAcceleration * Time.fixedDeltaTime;
                parallelSpeed = Mathf.Min(parallelSpeed, goalSpeed); // prevent overcorrection
            }
            else { // Slowing down
                if (slowingDv > goalSpeed - parallelSpeed) { // prevent overcorrection
                    parallelSpeed = goalSpeed;
                }
                else {
                    parallelSpeed -= slowingDv;
                }
            }

            // Velocity perpendicular to the goal direction is always slowing down
            var perpSign = Mathf.Sign(perpendicularSpeed);

            if (slowingDv > perpendicularSpeed * perpSign) {
                perpendicularSpeed = 0;
            }
            else {
                perpendicularSpeed -= slowingDv * perpSign;
            }


            // Combine all components of velocity
            _character.Velocity =
                parallelSpeed * goalWalkingDir
                + perpendicularSpeed * perpendicularDir
                + normalVelocity * _character.GroundNormal;

            // Apply gravity
            //if(GetComponent<CharacterController>())
            //if(!_character.isOnGround) {
            _character.Velocity += Physics.gravity * Time.fixedDeltaTime;
            //}
        }


        private Vector3 GetGoalDir () {
            var goalWalkingDir = new Vector3(WalkingDir.x, 0f, WalkingDir.y);
            goalWalkingDir -= Vector3.Dot(goalWalkingDir, _character.GroundNormal) * _character.GroundNormal;
            goalWalkingDir = goalWalkingDir.normalized;
            return goalWalkingDir;
        }

        private float GetGoalSpeed () {
            var walkMagnitude = Mathf.Min(WalkingDir.magnitude, 1);
            var goalSpeed = walkMagnitude * MaxSpeed;
            return goalSpeed;
        }


        // 
        // public-facing stuff

        public void SetDir (Vector2 heading) { WalkingDir = heading; }

        public void SetDir (Vector3 heading) { WalkingDir = new Vector2(heading.x, heading.z); }
    }
}