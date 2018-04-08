using System.Collections.Generic;

namespace Code.Doods.AI {
	public abstract class Composite : BehaviorTreeNode {
		protected List<BehaviorTreeNode> _children = new List<BehaviorTreeNode> ();

		protected Composite (Dood dood) : base (dood) { }

		public void AddChild (BehaviorTreeNode node)
		{
			_children.Add (node);
		}
	}


	public class Selector : Composite {
		int _current;

		public Selector (Dood dood) : base (dood) { }

		public override void OnInitialize ()
		{
			_current = 0;
		}

		protected override Status Update ()
		{
			// todo we need to make sure to shut things down?
			while (true) {
				Status status = _children [_current].Tick ();
				if (status != Status.Failure) { return status; }
				if (++_current == _children.Count) { return Status.Failure; }
			}
		}
	}

	public class Sequence : Composite {
		int _current;

		public Sequence (Dood dood) : base (dood) { }

		public override void OnInitialize ()
		{
			_current = 0;
		}

		protected override Status Update ()
		{
			while (true) {
				Status status = _children [_current].Tick ();
				if (status != Status.Success) { return status; }
				if (++_current == _children.Count) { return Status.Success; }
			}
		}
	}
}