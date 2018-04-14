using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (FlockBehaviour))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		public Characters.Walk Walk;
		public Characters.Character Character;
		public float minForce = .5f;
		FlockBehaviour _flock;
		public int Happiness;

		public void Initialize ()
		{
			Behavior = GetComponent<BehaviorTree> ().Root;
		}

		void Start ()
		{
			Walk = GetComponent<Characters.Walk> ();
			Character = GetComponent<Characters.Character> ();
			_flock = GetComponent<FlockBehaviour> ();
		}

		public bool MoveTowards (Vector3 pos, float thresh = 3f)
		{
			if (Vector3.Distance (pos, transform.position) < thresh) {
				Walk.SetDir (Vector3.zero);
				return true;
			}
			var direction = (pos - transform.position).normalized;
			Vector3 force = _flock.CalculateForce ();
			force = force * Time.deltaTime + direction;
			if (force.sqrMagnitude < minForce * minForce) {
				Walk.SetDir (Vector3.zero);
			} else {
				Walk.SetDir (force);
			}
			return false;
		}

		public void StopMoving ()
		{
			//_flock.SetDir (Vector2.zero);
		}
	}
}