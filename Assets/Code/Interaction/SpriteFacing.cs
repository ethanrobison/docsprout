using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFacing : MonoBehaviour {
	// Make sure sprite is facing camera (i.e. when getting dood status)

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation (-Camera.main.transform.forward, Camera.main.transform.up);
	}
}
