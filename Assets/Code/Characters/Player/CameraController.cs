using Code.Environment;
using UnityEngine;

namespace Code.Characters.Player
{
    public class CameraController : MonoBehaviour
    {
        private const float X_SENSITIVITY = 100f, Y_SENSITIVITY = 50f;
        private const float MIN_Y_ANGLE = -20f, MAX_Y_ANGLE = 50f;

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

        private Quaternion _camRotX = Quaternion.identity;
        private float _camRotY = 30f;
        private float _camDist = FOLLOW_DISTANCE;

        private ScreenDoorTransparency _lastSet;
        private readonly RaycastHit[] _alphaHits = new RaycastHit[8];
        private readonly Collider[] _overlaps = new Collider[4];


        private void Start () {
            _target = transform;

            var goalrotation = _camRotX * Quaternion.AngleAxis(_camRotY, Vector3.right);
            var goaldistance = CalculateCameraDistance(goalrotation * Vector3.back);
            var goalposition = _target.position - goalrotation * Vector3.forward * goaldistance;

            Camera.transform.position = goalposition;
            Camera.transform.rotation = goalrotation;
        }

        private void Update () {
            HandleZoomZones();
            HandleScreenDoors();
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


        private void HandleZoomZones () {
            float goalDist = FOLLOW_DISTANCE;

            int count = Physics.OverlapSphereNonAlloc(_target.position, COLLISION_RADIUS, _overlaps, CameraZoomZone);
            for (int i = 0; i < count; ++i) {
                var zoomzone = _overlaps[i].gameObject.GetComponent<CameraZoomZone>();
                if (zoomzone != null) { goalDist = Mathf.Min(zoomzone.CamDist, goalDist); }
            }

            _camDist = Mathf.Lerp(_camDist, goalDist, ZOOM_ZONE_SPEED * Time.deltaTime);
        }

        private void HandleScreenDoors () {
            var dist = 0f;
            ScreenDoorTransparency sdt = null;
            int count;
            count = Physics.OverlapSphereNonAlloc(Camera.transform.position, COLLISION_RADIUS, _overlaps);
            for (int i = 0; i < count; ++i) {
                sdt = _overlaps[i].gameObject.GetComponent<ScreenDoorTransparency>();
                if (sdt != null) { break; }
            }

            if (sdt == null) {
#if UNITY_EDITOR
                Debug.DrawLine(Camera.transform.position, Camera.transform.position +
                                                          Camera.transform.forward *
                                                          Vector3.Distance(Camera.transform.position,
                                                              _target.position));
#endif
                count = Physics.RaycastNonAlloc(Camera.transform.position, Camera.transform.forward, _alphaHits,
                    Vector3.Distance(Camera.transform.position, _target.position), ~ObscuresCamera.value,
                    QueryTriggerInteraction.Ignore);

                for (int i = 0; i < count; ++i) {
                    sdt = _alphaHits[i].collider.gameObject.GetComponent<ScreenDoorTransparency>();
                    if (sdt == null) { continue; }

                    dist = _alphaHits[i].distance;
                    break;
                }
            }

            if (_lastSet != null) { _lastSet.Alpha = 1f; }

            if (sdt != null) { sdt.Alpha = dist / (ALPHA_SLOPE + 0.0001f); }

            _lastSet = sdt;
        }

        // calculates the maximum allowed distance between the player and the camera, accounting for obstacles between them
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