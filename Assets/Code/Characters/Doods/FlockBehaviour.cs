using UnityEngine;


namespace Code.Doods {
	//[RequireComponent (typeof (Characters.Walk))]
	public class FlockBehaviour : Characters.Walk {

		public float NeighborhoodRadius = 5f;
		public float Damping = 1f;
		public float AttractWeight = 1f;
		public float RepelWeight = 1f;
		public float AlignWeight = 1f;
		public float AvoidWeight = 1f;

		[HideInInspector] public bool IsFlocking = true;

		Dood _dood;

		protected override void Start ()
		{
			base.Start();
			_dood = GetComponent<Dood> ();
		}

		protected override void Move ()
		{
			Vector2 force;
			Vector2 dir = WalkingDir;
			if(IsFlocking) {
				force = CalculateForce()*Time.fixedDeltaTime;
				SetDir(force + dir);
			}
			base.Move ();
			if(IsFlocking) {
				SetDir(dir);
			}
		}

		public Vector2 CalculateForce ()
		{
			Vector3 center = Vector3.zero;
			int numNearby = 0;
			Vector3 force = Vector3.zero;
			float sqrRadius = NeighborhoodRadius * NeighborhoodRadius;
			Vector3 temp;
			foreach (Dood dood in Game.Ctx.Doods.DoodList) {
				if (dood == _dood) continue;
				Vector3 diff = dood.transform.position - transform.position;
				if (diff.sqrMagnitude < sqrRadius) {
					center += dood.transform.position;
					++numNearby;
					diff /= diff.sqrMagnitude;
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
				return Vector3.zero;
			}
			center /= numNearby;
			temp = (center - transform.position) * AttractWeight;
			//if(temp.sqrMagnitude > sqrMinForce) {
			force += temp;
			//}

			force -= _dood.Character.velocity * Damping;

			return new Vector2(force.x, force.z);
		}
	}
}
