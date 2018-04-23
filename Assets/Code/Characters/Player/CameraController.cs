using Code.Environment;
using UnityEngine;

namespace Code.Characters.Player {
	public class CameraController : MonoBehaviour {

		public new Camera camera;
		public Transform target;
		float camRotY = 30f;
		Quaternion camRotX;

		public LayerMask obscuresCamera;

		public LayerMask CameraZoomZone;
		public float ZoomZoneSpeed = 2f;

		public float xSensitivity = 100f;
		public float ySensitivity = 50f;
		public float followDistance = 16f;
		public float minYAngle = -20f;
		public float maxYAngle = 50f;
		public bool invertY = false;
		public float stiffness = 25f;
		public float AlphaSlope = 5f;

		public float CollisionRadius;

		//public AnimationCurve zoomCurve;

		RaycastHit [] _alphaHits;
		Collider [] _overlaps;

		// Use this for initialization
		void Start ()
		{
			if (target == null) target = transform;
			camRotX = Quaternion.identity;
//			Quaternion goalCamRot = target.rotation * Quaternion.AngleAxis (camRotX, Vector3.up);
			Quaternion goalCamRot = camRotX;
			goalCamRot *= Quaternion.AngleAxis (camRotY, Vector3.right);

			RaycastHit hit;
			float camDist = followDistance;
			if (Physics.SphereCast (target.position, .5f, -(goalCamRot * Vector3.forward), out hit, followDistance - .5f, obscuresCamera)) {
				camDist = hit.distance;
			}

			Vector3 goalCamPos = target.position - goalCamRot * Vector3.forward * camDist;


			camera.transform.position = goalCamPos;
			camera.transform.rotation = goalCamRot;

			_alphaHits = new RaycastHit [8];
			_overlaps = new Collider [4];

			_camDist = followDistance;

		}

		// Update is called once per frame
		public void moveCamera (float x, float y)
		{

			camRotX *= Quaternion.AngleAxis(x * xSensitivity, Vector3.up);
//			camRotX = camRotX % 360;

			float dYCam = y;
			if (!invertY) dYCam *= -1;

			camRotY += dYCam * ySensitivity;

			camRotY = Mathf.Clamp (camRotY, minYAngle, maxYAngle);

		}

		Environment.ScreenDoorTransparency lastSet;
		float _camDist;
		void Update ()
		{
			float goalDist = followDistance;
			int n = Physics.OverlapSphereNonAlloc (target.position, CollisionRadius, _overlaps, CameraZoomZone);
			for (int i = 0; i < n; ++i) {
				CameraZoomZone ccz = _overlaps [i].gameObject.GetComponent<CameraZoomZone> ();
				if(ccz) {
					goalDist = Mathf.Min(ccz.CamDist, goalDist);
				}
			}

			if(_camDist < goalDist) {
				_camDist = Mathf.Min(_camDist + ZoomZoneSpeed*Time.deltaTime, goalDist);
			}
			if(_camDist > goalDist) {
				_camDist = Mathf.Max(_camDist - ZoomZoneSpeed*Time.deltaTime, goalDist);
			}
			Environment.ScreenDoorTransparency sdt = null;
			float dist = 0f;
			n = Physics.OverlapSphereNonAlloc (camera.transform.position, CollisionRadius, _overlaps);
			for (int i = 0; i < n; ++i) {
				
				if (sdt = _overlaps [i].gameObject.GetComponent<Environment.ScreenDoorTransparency> ()) {
					dist = 0f;
					break;
				}
			}
			if (sdt == null) {
#if UNITY_EDITOR
				Debug.DrawLine (camera.transform.position, camera.transform.position +
						   camera.transform.forward *
						   Vector3.Distance (camera.transform.position, target.position));
#endif
				n = Physics.RaycastNonAlloc (camera.transform.position, camera.transform.forward, _alphaHits,
												  Vector3.Distance (camera.transform.position, target.position), ~obscuresCamera.value,
												  QueryTriggerInteraction.Ignore);

				for (int i = 0; i < n; ++i) {
					if (sdt = _alphaHits [i].collider.gameObject.GetComponent<Environment.ScreenDoorTransparency> ()) {
						dist = _alphaHits [i].distance;
						break;
					}
				}
			}
			if (lastSet) {
				lastSet.Alpha = 1f;
			}
			if (sdt) {
				sdt.Alpha = dist / (AlphaSlope + 0.0001f);
			}
			lastSet = sdt;

		}

		void FixedUpdate ()
		{
//			Quaternion goalCamRot = target.rotation * Quaternion.AngleAxis (camRotX, Vector3.up);
			Quaternion goalCamRot = camRotX;
			goalCamRot *= Quaternion.AngleAxis (camRotY, Vector3.right);

			RaycastHit hit;
			float camDist = _camDist;
			if (Physics.SphereCast (target.position, CollisionRadius, -(goalCamRot * Vector3.forward), out hit, camDist - CollisionRadius, obscuresCamera, QueryTriggerInteraction.Ignore)) {
				camDist = hit.distance;
			}

			Vector3 goalCamPos = target.position - goalCamRot * Vector3.forward * camDist;


			camera.transform.position = Vector3.Lerp (camera.transform.position, goalCamPos, stiffness * Time.fixedDeltaTime);
			camera.transform.rotation = Quaternion.Slerp (camera.transform.rotation, goalCamRot, stiffness * Time.fixedDeltaTime);



		}
		
		public void AcceptControl(Quaternion x, float y) {
			camRotX = x;
			camRotY = y;
		}
	}

}
