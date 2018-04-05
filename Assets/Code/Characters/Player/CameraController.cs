using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera camera;
    float camRotY;
    float camRotX;

    [System.Serializable]
    public struct CameraSettings {
        public float xSensitivity;
        public float ySensitivity;
        public float followDistance;
        public float minYAngle;
        public float maxYAngle;
        public bool invertY;
        public float stiffness;

    }

    public CameraSettings cameraSettings;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void moveCamera (float x, float y) {

        camRotX += x*cameraSettings.xSensitivity;
        camRotX = camRotX % 360;

        float dYCam = y;
        if(!cameraSettings.invertY) dYCam *= -1;

        camRotY += dYCam*cameraSettings.ySensitivity;

        camRotY = Mathf.Clamp(camRotY, cameraSettings.minYAngle, cameraSettings.maxYAngle);
		
	}

    void FixedUpdate()
    {
        Quaternion goalCamRot = Quaternion.AngleAxis(camRotX, Vector3.up);
        goalCamRot *= Quaternion.AngleAxis(camRotY, Vector3.right);

        Vector3 goalCamPos = transform.position - goalCamRot*Vector3.forward*cameraSettings.followDistance;

        camera.transform.position = Vector3.Lerp(camera.transform.position, goalCamPos, cameraSettings.stiffness);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, goalCamRot, cameraSettings.stiffness);

    }
}
