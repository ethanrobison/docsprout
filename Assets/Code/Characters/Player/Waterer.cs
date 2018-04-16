using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterer : MonoBehaviour {

	public Transform SpherePos;
	public float Radius;

	void Start ()
	{
		
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Joystick1Button18)) { //X
			Collider [] cols = Physics.OverlapSphere (SpherePos.position, Radius);
			Waterable waterable;
			foreach (Collider col in cols) {
				waterable = col.gameObject.GetComponent<Waterable> ();
				if (waterable) {
					waterable.IncrMeter ();
				}
			}
		}
	}

}
