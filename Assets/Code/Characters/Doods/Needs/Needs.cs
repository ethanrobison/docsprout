using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needs : MonoBehaviour {

	public Code.Doods.Dood dood;
	public int NeedMeter; // 0-100
	public int [] NeedRange; // range that the dood needs to be c:
	public int NeedIncr; // give need
	public int NeedDecr; // neglect need
	public int HappIncr;
	public int HappDecr;

	// Use this for initialization
	protected virtual void Start ()
	{
		dood = GetComponent<Code.Doods.Dood> ();
		//NeedMeter = 75;
		//NeedRange = new int [2];
		//NeedRange [0] = 30;
		//NeedRange [1] = 90;
		//NeedIncr = 30;
		//NeedDecr = 20;
		//HappIncr = 20;
		//HappDecr = 10;
	}

	public void IncrMeter ()
	{
		NeedMeter = Mathf.Min (NeedMeter + NeedIncr, 100);
	}

	public void DecrMeter ()
	{
		NeedMeter = Mathf.Max (NeedMeter - NeedDecr, 0);
	}

	public void IncrHappiness ()
	{
		if (NeedRange [0] <= NeedMeter && NeedMeter <= NeedRange [1]) {
			dood.Happiness += HappIncr;
		}
	}

	public void DecrHappiness ()
	{
		if (NeedRange [0] >= NeedMeter || NeedMeter >= NeedRange [1]) {
			dood.Happiness += HappIncr;
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

}
