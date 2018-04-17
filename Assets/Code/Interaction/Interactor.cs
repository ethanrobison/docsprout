using UnityEngine;

/// <summary>
///  the interactor that approaches or departs IApproachable go's
/// </summary>
public class Interactor : MonoBehaviour {
	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<IApproachable> () != null) {
			other.GetComponent<IApproachable> ().OnApproach ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.GetComponent<IApproachable> () != null) {
			other.GetComponent<IApproachable> ().OnDepart ();
		}
	}
}
