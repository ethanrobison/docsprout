using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodStatus : MonoBehaviour {

	public Code.Doods.Dood dood;

	public float Happiness;
	private float HappMin;
	private float HappMax;
	private float HappSubtotal;

	public Waterable Waterable;
	private float WaterMeter;
	private float [] WaterRange;

	Code.Doods.DoodColor _doodColor;
	// Use this for initialization
	void Start ()
	{
		dood = GetComponent<Code.Doods.Dood> ();
		_doodColor = GetComponent<Code.Doods.DoodColor>();

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
			Happiness += 5f*Time.deltaTime;
		} else { Happiness -= 5f*Time.deltaTime; }

		if (Happiness <= HappMin) {
			Happiness = HappMin;
		} else if (Happiness >= HappMax) {
			Happiness = HappMax;
		}

	}

	// Update is called once per frame
	void Update ()
	{
		WaterMeter = Waterable.NeedMeter;
		CalcHapp ();
		if(_doodColor) {
			_doodColor.Happiness = Happiness/HappMax;
		}
		//Code.Utils.Logging.Log ("WaterMeter: " + WaterMeter.ToString ());
		//Code.Utils.Logging.Log ("Happiness: " + Happiness.ToString ());
	}
}
