namespace Code.Characters.Doods.AI
{
    public abstract class Decorator : BehaviorTreeNode
    {
        protected BehaviorTreeNode Child;

        protected Decorator (Dood dood) : base(dood) { }

        protected override Status Update () { return Status.Failure; }
    }

    public class AlwaysSucceed : Decorator
    {
        public AlwaysSucceed (Dood dood) : base(dood) { }

        protected override Status Update () {
            if (Child != null) Child.Tick();
            return Status.Success;
        }

        public void SetChild (BehaviorTreeNode child) { Child = child; }
    }
}