namespace Code.Doods.AI {
	public enum Status {
		Invalid,
		Running,
		Success,
		Failure
	}


	public abstract class BehaviorTreeNode {
		Status _status;


		public abstract void OnInitialize ();
		public abstract void OnTerminate (Status result);

		protected abstract Status Update ();

		public Status Tick ()
		{
			if (_status != Status.Running) OnInitialize ();
			_status = Update ();
			if (_status != Status.Running) OnTerminate (_status);
			return _status;
		}
	}
}
