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

			//var seq = new Sequence (dood);
			//seq.AddChild (follow);
			//seq.AddChild (new LogMessage ("Hello", dood));
			//seq.AddChild (new Idle (dood));

			var sel = new Selector (dood);
			//sel.AddChild (follow);
			//sel.AddChild (new Idle (dood));

			var close = new PlayerDistance (dood, -1f, 5f);
			close.AddToEnd (new LogMessage (dood, "close"));

			var medium = new PlayerDistance (dood, 5f, 15f);
			medium.AddToEnd (new LogMessage (dood, "medium"));

			var far = new PlayerDistance (dood, 15f, float.PositiveInfinity);
			far.AddToEnd (new LogMessage (dood, "far"));

			sel.AddToEnd (close);
			sel.AddToEnd (medium);
			sel.AddToEnd (far);

			Root = new Root (dood);
			Root.SetChild (sel);
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
