using Code.Characters.Player;
using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (FlockBehaviour))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		public Characters.Movement.Walk Walk;
		public Characters.Character Character;
		public float minForce = .5f;
		FlockBehaviour _flock;

		public void Initialize ()
		{
			var follow = new FollowTarget<Player> (this,
												   LayerMask.GetMask ("Player"),
												   3);
			int layermask = LayerMask.GetMask ("Player");

			var seq = new Sequence (this);
			seq.AddChild (follow);
			seq.AddChild (new LogMessage ("Hello", this));
			seq.AddChild (new Idle (this));

			var sel = new Selector (this);
			sel.AddChild (follow);
			sel.AddChild (new Idle (this));

			Behavior = new Root (sel, this);
		}

		void Start ()
		{
			Walk = GetComponent<Characters.Movement.Walk> ();
			Character = GetComponent<Characters.Character> ();
			_flock = GetComponent<FlockBehaviour> ();
		}

		void Update ()
		{
			var status = Behavior.Tick ();
			switch (status) {
			case Status.Invalid:
				break;
			case Status.Running:
				break;
			case Status.Success:
				break;
			case Status.Failure:
				break;
			}
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