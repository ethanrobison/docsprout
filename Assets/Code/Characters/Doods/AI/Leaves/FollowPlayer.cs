namespace Code.Doods.AI {
	public class FollowPlayer : BehaviorTreeNode {
		float _goalDistance;

		protected FollowPlayer (Dood dood, float distance) : base (dood)
		{
			_goalDistance = distance;
		}

		protected override Status Update ()
		{
			return Status.Failure;
		}
	}
}