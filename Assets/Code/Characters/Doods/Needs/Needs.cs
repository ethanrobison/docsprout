using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needs : MonoBehaviour {

	public Code.Doods.Dood dood;
	private float NeedMeterMin;
	private float NeedMeterMax;
	public float NeedMeter;
	public float [] NeedRange; // range that the dood needs to be c:
	public float NeedIncr; // give need
	public float NeedDecr; // neglect need

	// Use this for initialization
	protected virtual void Start ()
	{
		NeedMeterMin = 0f;
		NeedMeterMax = 100f;
		dood = GetComponent<Code.Doods.Dood> ();
	}

	public void IncrMeter ()
	{
		NeedMeter = Mathf.Min (NeedMeter + NeedIncr, NeedMeterMax);
	}

	public void DecrMeter (float time)
	{
		NeedMeter = Mathf.Max (NeedMeter - NeedDecr*time, NeedMeterMin);
	}

	// Update is called once per frame
	void Update ()
	{
		DecrMeter (Time.deltaTime);
	}

}
