using Code;
using Code.Session;
using Code.Utils;
using UnityEngine;

public class Waterer : MonoBehaviour {

	public Transform SpherePos;
	public float Radius;

	void Start ()
	{
		Game.Sesh.Input.Monitor.RegisterMapping (ControllerButton.XButton, WaterNearby);
	}


	void WaterNearby ()
	{
		Collider [] cols = Physics.OverlapSphere (SpherePos.position, Radius);
		foreach (Collider col in cols) {
			var waterable = col.gameObject.GetComponent<Waterable> ();
			if (waterable != null) { waterable.IncrMeter (); }
		}
	}
}
