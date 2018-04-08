namespace Code.Doods.AI {
	public class Idle : BehaviorTreeNode {
		public override void OnInitialize () { }

		public override void OnTerminate (Status result) { }

		protected override Status Update ()
		{
			return Status.Success;
		}
	}
}
