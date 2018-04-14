using Code.Utils;

namespace Code.Doods.AI {
	public class LogMessage : BehaviorTreeNode {
		readonly string _message;

		public LogMessage (string message, Dood dood) : base (dood)
		{
			_message = message;
		}

		protected override Status Update ()
		{
			Logging.Log (_message);
			return Status.Success;
		}
	}
}