using System.Collections;
using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (FlockBehaviour))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		public Characters.Walk Walk;
		public Characters.Character Character;
		FlockBehaviour _flock;


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

		public bool MoveTowards (Vector3 pos, float thresh = 5f)
		{
			if (Vector3.Distance (pos, transform.position) < thresh) {
				if(finishedMove) {
					Walk.SetDir (Vector3.zero);
					return true;	
				}
				if(!finishedMove && !isTiming) {
					StartCoroutine(stopTimer());
				}
			}
			if(finishedMove) {
				finishedMove = false;
			}
			var direction = (pos - transform.position).normalized;
			Vector3 force = _flock.CalculateForce ();
			force = force * Time.deltaTime + direction;
			Walk.SetDir (force);
			return false;
		}

		bool finishedMove;
		bool isTiming;
		IEnumerator stopTimer() {
			isTiming = true;
			yield return new WaitForSeconds(1.5f);
			isTiming = false;
			finishedMove = true;
		}

		public void StopMoving ()
		{
			//_flock.SetDir (Vector2.zero);
		}
	}
}