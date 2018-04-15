using Code.Characters.Player;
using UnityEngine;

namespace Code.Doods.AI {
	public class FollowPlayer : BehaviorTreeNode {
		float _goalDistance;
		Player _player;

		public FollowPlayer (Dood dood, float distance = 5f) : base (dood)
		{
			_goalDistance = distance;
			_player = Game.Ctx.Player;
		}

		public override void OnTerminate (Status result)
		{
			base.OnTerminate (result);
			_dood.StopMoving ();
		}

		protected override Status Update ()
		{
			var dist = Vector3.Distance (_dood.transform.position, _player.transform.position);
			if (dist < _goalDistance) { return Status.Success; }
			_dood.MoveTowards (_player.transform.position);
			return Status.Running;
		}
	}
}