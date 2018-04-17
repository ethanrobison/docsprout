using Code.Doods;
using UnityEngine;

public class GetDoodStatus : MonoBehaviour, IApproachable {

	// get a dood's status when you approach it

	public Code.Doods.Dood dood;
	private Waterable Waterable;

	public GameObject StatusDisplay;

	public GameObject NeedWater;
	public GameObject StopWater;

	void IApproachable.OnApproach() {
		StatusDisplay.SetActive (true);
		// display happiness meter
	}

	void IApproachable.OnDepart() {
		StatusDisplay.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		Waterable = dood.GetComponent<Waterable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Waterable) {
			if (Waterable.NeedMeter < Waterable.NeedRange [0]) {
				NeedWater.SetActive (true);
				StopWater.SetActive (false);
			} else if (Waterable.NeedMeter > Waterable.NeedRange [1]) {
				StopWater.SetActive (true);
				NeedWater.SetActive (false);
			} else {
				NeedWater.SetActive (false);
				StopWater.SetActive (false);
			}
		}
	}
}
