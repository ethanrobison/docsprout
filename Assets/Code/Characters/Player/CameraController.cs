using Code.Environment;
using UnityEngine;

namespace Code.Characters.Player
{
    public class CameraController : MonoBehaviour
    {
        private const float X_SENSITIVITY = 100f;
        private const float Y_SENSITIVITY = 50f;
        private const float MIN_Y_ANGLE = -20f;
        private const float MAX_Y_ANGLE = 50f;

        private const float ZOOM_ZONE_SPEED = 2f;
        private const float FOLLOW_DISTANCE = 16f;
        private const float STIFFNESS = 25f;
        private const float COLLISION_RADIUS = 0.25f;
        private const float ALPHA_SLOPE = 25f;


        public Camera Camera;
        public LayerMask ObscuresCamera;
        public bool InvertY;
        public LayerMask CameraZoomZone;

        private Transform _target;
        private float _camRotY = 30f;
        private Quaternion _camRotX;
        private ScreenDoorTransparency _lastSet;
        private float _camDist;
        private readonly RaycastHit[] _alphaHits = new RaycastHit[8];
        private readonly Collider[] _overlaps = new Collider[4];


        private void Start () {
            _target = transform;
            _camRotX = Quaternion.identity;
            Quaternion goalCamRot = _camRotX;
            goalCamRot *= Quaternion.AngleAxis(_camRotY, Vector3.right);

            RaycastHit hit;
            float camDist = FOLLOW_DISTANCE;
            if (Physics.SphereCast(_target.position, .5f, -(goalCamRot * Vector3.forward), out hit,
                FOLLOW_DISTANCE - .5f,
                ObscuresCamera)) {
                camDist = hit.distance;
            }

            Vector3 goalCamPos = _target.position - goalCamRot * Vector3.forward * camDist;

            Camera.transform.position = goalCamPos;
            Camera.transform.rotation = goalCamRot;

            _camDist = FOLLOW_DISTANCE;
        }

        private void Update () {
            float goalDist = FOLLOW_DISTANCE;
            int n = Physics.OverlapSphereNonAlloc(_target.position, COLLISION_RADIUS, _overlaps, CameraZoomZone);
            for (int i = 0; i < n; ++i) {
                CameraZoomZone ccz = _overlaps[i].gameObject.GetComponent<CameraZoomZone>();
                if (ccz) {
                    goalDist = Mathf.Min(ccz.CamDist, goalDist);
                }
            }

            if (_camDist < goalDist) {
                _camDist = Mathf.Min(_camDist + ZOOM_ZONE_SPEED * Time.deltaTime, goalDist);
            }

            if (_camDist > goalDist) {
                _camDist = Mathf.Max(_camDist - ZOOM_ZONE_SPEED * Time.deltaTime, goalDist);
            }

            ScreenDoorTransparency sdt = null;
            float dist = 0f;
            n = Physics.OverlapSphereNonAlloc(Camera.transform.position, COLLISION_RADIUS, _overlaps);
            for (int i = 0; i < n; ++i) {
                if (sdt = _overlaps[i].gameObject.GetComponent<ScreenDoorTransparency>()) {
                    dist = 0f;
                    break;
                }
            }

            if (sdt == null) {
#if UNITY_EDITOR
                Debug.DrawLine(Camera.transform.position, Camera.transform.position +
                                                          Camera.transform.forward *
                                                          Vector3.Distance(Camera.transform.position,
                                                              _target.position));
#endif
                n = Physics.RaycastNonAlloc(Camera.transform.position, Camera.transform.forward, _alphaHits,
                    Vector3.Distance(Camera.transform.position, _target.position), ~ObscuresCamera.value,
                    QueryTriggerInteraction.Ignore);

                for (int i = 0; i < n; ++i) {
                    if (sdt = _alphaHits[i].collider.gameObject.GetComponent<ScreenDoorTransparency>()) {
                        dist = _alphaHits[i].distance;
                        break;
                    }
                }
            }

            if (_lastSet) {
                _lastSet.Alpha = 1f;
            }

            if (sdt) {
                sdt.Alpha = dist / (ALPHA_SLOPE + 0.0001f);
            }

            _lastSet = sdt;
        }

        private void FixedUpdate () {
            var goalrotation = _camRotX * Quaternion.AngleAxis(_camRotY, Vector3.right);
            var goaldirection = goalrotation * Vector3.forward;

            var cappeddistance = CalculateCameraDistance(-goaldirection);
            var goalposition = _target.position - goaldirection * cappeddistance;

            Camera.transform.position =
                Vector3.Lerp(Camera.transform.position, goalposition, STIFFNESS * Time.fixedDeltaTime);
            Camera.transform.rotation =
                Quaternion.Slerp(Camera.transform.rotation, goalrotation, STIFFNESS * Time.fixedDeltaTime);
        }

        // calculates the maximum distance between the player and the camera, accounting for obstacles between them
        private float CalculateCameraDistance (Vector3 direction) {
            RaycastHit hit;
            return Physics.SphereCast(_target.position, COLLISION_RADIUS, direction, out hit,
                _camDist - COLLISION_RADIUS, ObscuresCamera, QueryTriggerInteraction.Ignore)
                ? hit.distance
                : _camDist;
        }

        //
        // public api

        public void MoveCamera (float x, float y) {
            _camRotX *= Quaternion.AngleAxis(x * X_SENSITIVITY, Vector3.up);

            float dYCam = y;
            if (!InvertY) dYCam *= -1;

            _camRotY += dYCam * Y_SENSITIVITY;

            _camRotY = Mathf.Clamp(_camRotY, MIN_Y_ANGLE, MAX_Y_ANGLE);
        }

        public void AcceptControl (Quaternion x, float y) {
            _camRotX = x;
            _camRotY = y;
        }
    }
}