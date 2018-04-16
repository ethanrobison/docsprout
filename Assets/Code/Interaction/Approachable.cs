using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approachable : MonoBehaviour, IApproachable {

	void IApproachable.OnApproach () {
		Code.Utils.Logging.Log ("Hi");
	}

	void IApproachable.OnDepart() {
		Code.Utils.Logging.Log ("Bye");
	}


	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

}

public interface IApproachable {
	void OnApproach ();
	void OnDepart ();
}
