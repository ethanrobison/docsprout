using UnityEngine;

namespace Code.Doods.AI {
	public class Idle : BehaviorTreeNode {
		public override void OnInitialize () { }

		public override void OnTerminate (Status result) { }

		protected override Status Update ()
		{
			Debug.Log ("Idling");
			return Status.Running; // todo should this be a "success?" probably
		}
	}
}
