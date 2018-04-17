using Code.Utils;

namespace Code.Characters.Doods.AI
{
    public class LogMessage : BehaviorTreeNode
    {
        private readonly string _message;

        public LogMessage (Dood dood, string message) : base(dood) { _message = message; }

        protected override Status Update () {
            Logging.Log(_message);
            return Status.Success;
        }
    }
}