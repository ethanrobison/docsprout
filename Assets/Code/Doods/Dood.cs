using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (FlockBehaviour))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		public Characters.Movement.Walk Walk;
		public Characters.Character Character;
		FlockBehaviour _flock;
		//public Vector3 MoveTowardsDir;

		public void Initialize ()
		{
			int layermask = LayerMask.GetMask ("Player");
			var follow = new FollowTarget<Characters.Player.Player> (this, layermask);


			var seq = new Sequence (this);
			seq.AddChild (follow);
			seq.AddChild (new LogMessage ("Hello", this));
			seq.AddChild (new Idle (this));

			var sel = new Selector (this);
			sel.AddChild (seq);
			sel.AddChild (new Idle (this));

			Behavior = new Root (sel, this);
		}

		void Start ()
		{
			Walk = GetComponent<Characters.Movement.Walk> ();
			Character = GetComponent<Characters.Character> ();
			_flock = GetComponent<FlockBehaviour>();
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


		public bool MoveTowards (Vector3 pos, float thresh = 2f)
		{
			if (Vector3.Distance (pos, transform.position) < thresh) { 
				Walk.SetDir (Vector3.zero);
				return true; 
			}
			var direction = (pos - transform.position).normalized;
			Walk.SetDir (direction);
			_flock.Tick();
			//MoveTowardsDir = direction;
			//MoveTowardsDir = Vector3.zero;
			return false;
		}
	}
}