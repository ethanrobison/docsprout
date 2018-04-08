using System.Collections.Generic;

namespace Code.Doods.AI {
	public abstract class Composite : BehaviorTreeNode {
		protected List<BehaviorTreeNode> _children = new List<BehaviorTreeNode> ();

		public void AddChild (BehaviorTreeNode node)
		{
			_children.Add (node);
		}
	}


	public class Selector : Composite {
		int _current;

		public override void OnInitialize ()
		{
			_current = 0;
		}

		public override void OnTerminate (Status result) { }

		protected override Status Update ()
		{
			while (true) {
				Status status = _children [_current].Tick ();
				if (status != Status.Failure) { return status; }
				if (++_current == _children.Count) { return Status.Failure; }
			}
		}
	}
}