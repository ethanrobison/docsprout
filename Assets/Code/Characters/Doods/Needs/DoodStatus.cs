using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodStatus : MonoBehaviour {

	public Code.Doods.Dood dood;

	public float Happiness;
	private float HappMin;
	private float HappMax;

	public Waterable Waterable;
	public float WaterMeter;
	public float [] WaterRange;

	// Use this for initialization
	void Start ()
	{
		dood = GetComponent<Code.Doods.Dood> ();

		HappMin = 0f;
		HappMax = 100f;
		//Happiness = 75f;

		Waterable = GetComponent<Waterable> ();
		WaterMeter = Waterable.NeedMeter;
		WaterRange = Waterable.NeedRange;
	}

	void CalcHapp ()
	{
		if (WaterRange [0] <= WaterMeter && WaterMeter <= WaterRange [1]) {
			WaterMeter += 5f;
		} else { WaterMeter -= 5f; }

		if (WaterMeter <= HappMin) {
			WaterMeter = HappMin;
		} else if (WaterMeter >= HappMax) {
			WaterMeter = HappMax;
		}

		Happiness = WaterMeter;
	}

	// Update is called once per frame
	void Update ()
	{
		CalcHapp ();
		Code.Utils.Logging.Log ("WaterMeter: " + WaterMeter.ToString ());
		Code.Utils.Logging.Log ("Happiness: " + Happiness.ToString ());
		Debug.Log ("HI");
	}
}
