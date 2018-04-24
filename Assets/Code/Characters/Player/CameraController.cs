using Code.Environment;
using UnityEngine;

namespace Code.Characters.Player
{
    public class CameraController : MonoBehaviour
    {
        public Camera Camera;
        float _camRotY = 30f;
        Quaternion _camRotX;

        public LayerMask ObscuresCamera;

        public LayerMask CameraZoomZone;
        public float ZoomZoneSpeed = 2f;
        public float XSensitivity = 100f;
        public float YSensitivity = 50f;
        public float FollowDistance = 16f;
        public float MinYAngle = -20f;
        public float MaxYAngle = 50f;
        public bool InvertY;
        public float Stiffness = 25f;
        public float AlphaSlope = 5f;
        public float CollisionRadius;

        public Transform Target;
        private ScreenDoorTransparency _lastSet;
        private float _camDist;
        private RaycastHit[] _alphaHits;
        private Collider[] _overlaps;

        private void Start () {
            if (Target == null) Target = transform;
            _camRotX = Quaternion.identity;
            Quaternion goalCamRot = _camRotX;
            goalCamRot *= Quaternion.AngleAxis(_camRotY, Vector3.right);

            RaycastHit hit;
            float camDist = FollowDistance;
            if (Physics.SphereCast(Target.position, .5f, -(goalCamRot * Vector3.forward), out hit, FollowDistance - .5f,
                ObscuresCamera)) {
                camDist = hit.distance;
            }

            Vector3 goalCamPos = Target.position - goalCamRot * Vector3.forward * camDist;


            Camera.transform.position = goalCamPos;
            Camera.transform.rotation = goalCamRot;

            _alphaHits = new RaycastHit [8];
            _overlaps = new Collider [4];

            _camDist = FollowDistance;
        }

        public void MoveCamera (float x, float y) {
            _camRotX *= Quaternion.AngleAxis(x * XSensitivity, Vector3.up);

            float dYCam = y;
            if (!InvertY) dYCam *= -1;

            _camRotY += dYCam * YSensitivity;

            _camRotY = Mathf.Clamp(_camRotY, MinYAngle, MaxYAngle);
        }


        private void Update () {
            float goalDist = FollowDistance;
            int n = Physics.OverlapSphereNonAlloc(Target.position, CollisionRadius, _overlaps, CameraZoomZone);
            for (int i = 0; i < n; ++i) {
                CameraZoomZone ccz = _overlaps[i].gameObject.GetComponent<CameraZoomZone>();
                if (ccz) {
                    goalDist = Mathf.Min(ccz.CamDist, goalDist);
                }
            }

            if (_camDist < goalDist) {
                _camDist = Mathf.Min(_camDist + ZoomZoneSpeed * Time.deltaTime, goalDist);
            }

            if (_camDist > goalDist) {
                _camDist = Mathf.Max(_camDist - ZoomZoneSpeed * Time.deltaTime, goalDist);
            }

            ScreenDoorTransparency sdt = null;
            float dist = 0f;
            n = Physics.OverlapSphereNonAlloc(Camera.transform.position, CollisionRadius, _overlaps);
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
                                                          Vector3.Distance(Camera.transform.position, Target.position));
#endif
                n = Physics.RaycastNonAlloc(Camera.transform.position, Camera.transform.forward, _alphaHits,
                    Vector3.Distance(Camera.transform.position, Target.position), ~ObscuresCamera.value,
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
                sdt.Alpha = dist / (AlphaSlope + 0.0001f);
            }

            _lastSet = sdt;
        }

        private void FixedUpdate () {
            Quaternion goalCamRot = _camRotX;
            goalCamRot *= Quaternion.AngleAxis(_camRotY, Vector3.right);

            RaycastHit hit;
            float camDist = _camDist;
            if (Physics.SphereCast(Target.position, CollisionRadius, -(goalCamRot * Vector3.forward), out hit,
                camDist - CollisionRadius, ObscuresCamera, QueryTriggerInteraction.Ignore)) {
                camDist = hit.distance;
            }

            Vector3 goalCamPos = Target.position - goalCamRot * Vector3.forward * camDist;


            Camera.transform.position =
                Vector3.Lerp(Camera.transform.position, goalCamPos, Stiffness * Time.fixedDeltaTime);
            Camera.transform.rotation =
                Quaternion.Slerp(Camera.transform.rotation, goalCamRot, Stiffness * Time.fixedDeltaTime);
        }

        public void AcceptControl (Quaternion x, float y) {
            _camRotX = x;
            _camRotY = y;
        }
    }
}