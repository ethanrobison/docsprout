using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCameraZone : MonoBehaviour {

	public float camDist = 5f;

	float prevCamDist;

	private void OnTriggerEnter (Collider other)
	{
		Code.Characters.Player.CameraController cam = other.GetComponent<Code.Characters.Player.CameraController> ();
		if (cam) {
			prevCamDist = cam.followDistance;
			cam.followDistance = camDist;
		}
	}

	private void OnTriggerExit (Collider other)
	{
		Code.Characters.Player.CameraController cam = other.GetComponent<Code.Characters.Player.CameraController> ();
		if (cam) {
			cam.followDistance = prevCamDist;
		}
	}
}
