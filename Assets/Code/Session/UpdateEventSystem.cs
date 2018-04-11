using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateEventSystem : MonoBehaviour {

	StandaloneInputModule inputModule;

	void Start ()
	{
		inputModule = GetComponent<StandaloneInputModule> ();
	}

	// Update is called once per frame
	void Update ()
	{
		// todo actually wait for a new controller
		//inputModule.horizontalAxis = "leftHorizontal" + Code.Game.Sesh.Input.GetButtonSuffix ();
		//inputModule.verticalAxis = "leftVertical" + Code.Game.Sesh.Input.GetButtonSuffix ();
		//inputModule.submitButton = "A" + Code.Game.Sesh.Input.GetButtonSuffix ();
		//inputModule.cancelButton = "B" + Code.Game.Sesh.Input.GetButtonSuffix ();
	}
}
