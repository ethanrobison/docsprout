using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class Need : MonoBehaviour {
		public Dood dood;
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
			dood = GetComponent<Dood> ();
		}

		public void IncrMeter ()
		{
			NeedMeter = Mathf.Min (NeedMeter + NeedIncr, NeedMeterMax);
		}

		public void DecrMeter (float time)
		{
			NeedMeter = Mathf.Max (NeedMeter - NeedDecr * time, NeedMeterMin);
		}

		// Update is called once per frame
		protected virtual void Update ()
		{
			DecrMeter (Time.deltaTime);
		}

		struct NeedValues {
			public float Min { get; private set; }
			public float Max { get; private set; }
			public float Meter { get; private set; }

		}
	}
}
