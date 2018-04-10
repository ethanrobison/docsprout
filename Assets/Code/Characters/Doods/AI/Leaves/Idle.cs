namespace Code.Doods.AI {
	public class Idle : BehaviorTreeNode {
		public Idle (Dood dood) : base (dood) { }

		protected override Status Update ()
		{
			return Status.Success;
		}
	}
}
