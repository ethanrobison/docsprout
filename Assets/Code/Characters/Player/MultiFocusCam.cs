using Code.Utils;
using UnityEngine;

namespace Code.Characters.Player
{
    public class MultiFocusCam : MonoBehaviour
    {
        private const float MIN_ANGLE = 15f;
        private const float MIN_DIST = 5f;
        private const float ANGLE_CHANGE = 1f;
        private const float STIFFNESS = 5f;
        private const float SCREEN_BORDER = 0.1f;

        private Transform _target;
        private Transform _player;

        private void Start () {
            _player = transform;
            _target = transform.Find("Cursor");
            Logging.Assert(_target != null, "Missing cursor!");
        }

        private void FixedUpdate () {
            var targetDir = _target.position - _player.position;
            var yAngle = CalculateYAngle(targetDir);

            Vector4 playerPos = _player.position;
            playerPos.w = 1f;
            playerPos = Camera.main.worldToCameraMatrix * playerPos;
            playerPos.y /= (1f - SCREEN_BORDER);

            var camDist = -1.2f * playerPos.y / Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f);
            camDist = Mathf.Max(MIN_DIST, camDist);
            camDist += Vector3.Dot(Camera.main.transform.forward, targetDir);

            var goalrotation = Quaternion.LookRotation(targetDir) * Quaternion.AngleAxis(yAngle, Vector3.right);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, goalrotation,
                STIFFNESS * Time.fixedDeltaTime);

            var goalposition = _target.position - camDist * Camera.main.transform.forward;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, goalposition,
                STIFFNESS * Time.fixedDeltaTime);
        }


        public void RelinquishControl (CameraController ctrl) {
            var targetDir = _target.position - _player.position;
            var yAngle = CalculateYAngle(targetDir);
            ctrl.OnAcceptControl(Quaternion.LookRotation(targetDir), yAngle);
        }

        private static float CalculateYAngle (Vector3 direction) {
            direction.y = 0;
            var distance = direction.magnitude;
            return 90f - (90f - MIN_ANGLE) * distance / (distance + ANGLE_CHANGE);
        }
    }
}