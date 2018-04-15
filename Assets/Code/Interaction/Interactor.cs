using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour {

	/// <summary>
	///  the interactor that approaches or departs IApproachable go's
	/// </summary>

	private void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<IApproachable>() != null) {
			other.GetComponent<IApproachable> ().OnApproach ();
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if (other.GetComponent<IApproachable> () != null) {
			other.GetComponent<IApproachable> ().OnDepart ();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
