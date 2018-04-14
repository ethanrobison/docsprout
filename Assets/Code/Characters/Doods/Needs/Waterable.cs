using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterable : Needs {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		NeedMeter = 75;
		NeedRange = new int [2];
		NeedRange [0] = 30;
		NeedRange [1] = 90;
		NeedIncr = 30;
		NeedDecr = 20;
		HappIncr = 20;
		HappDecr = 10;
	}
	
	// Update is called once per frame
	void Update () {
		WaterTheDood ();
	}

	void WaterTheDood() {
		if (Input.GetKeyDown(KeyCode.Joystick1Button18)){
			IncrMeter ();
			Code.Utils.Logging.Log ("NeedMeter: " + NeedMeter.ToString());
		}
	}
}
