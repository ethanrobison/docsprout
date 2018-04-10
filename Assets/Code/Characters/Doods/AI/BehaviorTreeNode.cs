namespace Code.Doods.AI {
	public enum Status {
		Invalid,
		Running,
		Success,
		Failure
	}


	public abstract class BehaviorTreeNode {
		Status _status;

		protected Dood _dood;
		protected Root _root { get { return _dood.Behavior; } }

		protected BehaviorTreeNode (Dood dood)
		{
			_dood = dood;
		}

		public virtual void OnInitialize () { }
		public virtual void OnTerminate (Status result) { }
		// force the node to wrap up (mostly makes sense for leaves)
		public virtual void Abort ()
		{
			if (_status != Status.Invalid) OnTerminate (Status.Failure);
		}

		protected abstract Status Update ();

		public Status Tick ()
		{
			if (_status != Status.Running) OnInitialize ();
			_status = Update ();
			if (_status != Status.Running) OnTerminate (_status);
			return _status;
		}
	}


	public class Root : BehaviorTreeNode {
		readonly BehaviorTreeNode _child;

		public Root (BehaviorTreeNode child, Dood dood) : base (dood)
		{
			_child = child;
		}

		protected override Status Update ()
		{
			return _child.Tick ();
		}
	}
}
