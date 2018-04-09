using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Flock))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		Flock _flock;

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
			_flock = GetComponent<Flock> ();
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


		public bool MoveTowards (Vector3 pos, float thresh = 1f)
		{
			if (Vector3.Distance (pos, transform.position) < thresh) { return true; }
			var direction = (pos - transform.position).normalized;
			_flock.SetDir (direction);
			return false;
		}
	}
}