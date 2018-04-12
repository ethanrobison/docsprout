using Code.Characters.Player;
using UnityEngine;

namespace Code.Doods.AI {
	[RequireComponent (typeof (Dood))]
	public class BehaviorTree : MonoBehaviour {
		public Root Root { get; private set; }

		void Start ()
		{
			var dood = GetComponent<Dood> ();
			var follow = new FollowTarget<Player> (dood,
												   LayerMask.GetMask ("Player"),
												   10);
			int layermask = LayerMask.GetMask ("Player");

			var seq = new Sequence (dood);
			seq.AddChild (follow);
			seq.AddChild (new LogMessage ("Hello", dood));
			seq.AddChild (new Idle (dood));

			var sel = new Selector (dood);
			sel.AddChild (follow);
			sel.AddChild (new Idle (dood));

			Root = new Root (sel, dood);
		}

		void Update ()
		{
			var status = Root.Tick ();
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
	}
}
