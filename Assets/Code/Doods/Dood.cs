using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Walk))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		Walk _walk;

		public void Initialize ()
		{
			int layermask = LayerMask.GetMask ("Player");
			var follow = new FollowTarget<Player> (this, layermask);

			var seq = new Sequence (this);
			seq.AddChild (follow);
			seq.AddChild (new LogMessage ("Hello", this));
			seq.AddChild (new Idle (this));
			Behavior = new Root (seq, this);
		}

		void Start ()
		{
			_walk = GetComponent<Walk> ();
		}

		void Update ()
		{
			var status = Behavior.Tick ();
			switch (status) {
			case Status.Invalid:
				Debug.Log ("Invalid");
				break;
			case Status.Running:
				Debug.Log ("Running");
				break;
			case Status.Success:
				Debug.Log ("Success");
				break;
			case Status.Failure:
				Debug.Log ("Failure");
				break;
			}
		}


		public bool MoveTowards (Vector3 pos, float thresh = 1f)
		{
			if (Vector3.Distance (pos, transform.position) < thresh) { return true; }
			var direction = (pos - transform.position).normalized;
			_walk.SetDir (direction);
			return false;
		}
	}
}