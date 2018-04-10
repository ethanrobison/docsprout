namespace Code.Doods.AI {
	// todo implement me
	public abstract class Decorator : BehaviorTreeNode {
		BehaviorTreeNode _child;

		protected Decorator (Dood dood) : base (dood) { }

		protected override Status Update ()
		{

			return Status.Failure;
		}
	}
}