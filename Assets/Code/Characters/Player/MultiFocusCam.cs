using UnityEngine;

namespace Code.Characters.Player
{
    public class MultiFocusCam : MonoBehaviour
    {
        private const float MIN_ANGLE = 15f;
        private const float MIN_DIST = 5f;
        private const float ANGLE_CHANGE = 1f;
        private const float STIFFNESS = 15f;

        public float ScreenBorder;
        public Transform Target;
        public Transform Player;

        private Quaternion _goalRot;
        private Vector3 _goalPos;

        private void FixedUpdate () {
            Vector3 targetDir = Target.position - Player.position;
            float targetY = targetDir.y;
            targetDir.y = 0f;
            float targetDist = targetDir.magnitude;
            float yAngle = 90f - (90f - MIN_ANGLE) * targetDist / (targetDist + ANGLE_CHANGE);
            _goalRot = Quaternion.LookRotation(targetDir) *
                       Quaternion.AngleAxis(yAngle, Vector3.right);

            Vector4 playerPos = Player.position;
            playerPos.w = 1f;
            playerPos = Camera.main.worldToCameraMatrix * playerPos;
            float camDist = -1.2f * playerPos.y / Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f);
            camDist = Mathf.Max(MIN_DIST, camDist);
            targetDir.y = targetY;
            camDist += Vector3.Dot(Camera.main.transform.forward, targetDir);

            _goalPos = Target.position - camDist * Camera.main.transform.forward;

            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, _goalRot,
                STIFFNESS * Time.fixedDeltaTime);

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _goalPos,
                STIFFNESS * Time.fixedDeltaTime);
        }


        public void RelinquishControl (CameraController ctrl) {
            Vector3 targetDir = Target.position - Player.position;
            targetDir.y = 0f;
            float targetDist = targetDir.magnitude;
            float yAngle = 90f - (90f - MIN_ANGLE) * targetDist / (targetDist + ANGLE_CHANGE);
            ctrl.OnAcceptControl(Quaternion.LookRotation(targetDir), yAngle);
        }
    }
}