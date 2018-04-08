namespace Code.Doods.AI {
	public class Idle : BehaviorTreeNode {
		protected override Status Update ()
		{
			return Status.Success;
		}
	}
}
