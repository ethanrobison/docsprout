using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Code.Doods {
	[RequireComponent (typeof (Characters.Movement.Walk))]
	public class FlockBehaviour : MonoBehaviour {

		public float NeighborhoodRadius = 5f;
		public float minForce = .1f;
		public float damping = 1f;
		public float AttractWeight = 1f;
		public float RepelWeight = 1f;
		public float AlignWeight = 1f;
		public float AvoidWeight = 1f;

		Dood _dood;

		// Use this for initialization
		void Start ()
		{
			_dood = GetComponent<Dood> ();
		}

		// Update is called once per frame
		public void Tick ()
		{
			//_dood.Walk.SetDir (_dood.MoveTowardsDir);
			Vector3 center = Vector3.zero;
			int numNearby = 0;
			Vector3 force = Vector3.zero;
			float sqrRadius = NeighborhoodRadius * NeighborhoodRadius;
			float sqrMinForce = minForce*minForce;
			Vector3 temp;
			foreach (Dood dood in Game.Ctx.Doods.DoodList) {
				if (dood == _dood) continue;
				Vector3 diff = dood.transform.position - transform.position;
				if (diff.sqrMagnitude < sqrRadius) {
					center += dood.transform.position;
					++numNearby;
					temp = diff * RepelWeight;
					//if(temp.sqrMagnitude > sqrMinForce) {
						force -= temp;
					//}
					temp = (dood.Character.velocity - _dood.Character.velocity) * AlignWeight;
					//if(temp.sqrMagnitude > sqrMinForce) {
						force += temp;
					//}
				}
			}
			if (numNearby == 0) {
				return;
			}
			center /= numNearby;
			temp = (center - transform.position) * AttractWeight;
			//if(temp.sqrMagnitude > sqrMinForce) {
				force += temp;
			//}

			force -= _dood.Character.velocity*damping;
			if(force.sqrMagnitude < sqrMinForce) {
				force = Vector3.zero;
			}

			_dood.Walk.AddDir (force*Time.deltaTime);
		}
	}
}
