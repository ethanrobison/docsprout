using Code.Doods;
using UnityEngine;

public class DoodStatus : MonoBehaviour {

	public Dood Dood;

	public float Happiness;
	public Waterable Waterable;

	float _minHappiness;
	float _maxHappiness;

	float _waterMeter;
	float [] _waterRange;

	DoodColor _doodColor;

	void Start ()
	{
		Dood = GetComponent<Dood> ();
		_doodColor = GetComponent<DoodColor> ();

		_minHappiness = 0f;
		_maxHappiness = 100f;
		//Happiness = 75f;

		Waterable = GetComponent<Waterable> ();
		_waterMeter = Waterable.NeedMeter;
		_waterRange = Waterable.NeedRange;
	}

	void CalcHapp ()
	{
		if (_waterRange [0] <= _waterMeter && _waterMeter <= _waterRange [1]) {
			Happiness += 5f * Time.deltaTime;
		} else { Happiness -= 5f * Time.deltaTime; }

		if (Happiness <= _minHappiness) {
			Happiness = _minHappiness;
		} else if (Happiness >= _maxHappiness) {
			Happiness = _maxHappiness;
		}
	}

	void Update ()
	{
		_waterMeter = Waterable.NeedMeter;
		CalcHapp ();
		if (_doodColor) {
			_doodColor.Happiness = Happiness / _maxHappiness;
		}
	}
}
