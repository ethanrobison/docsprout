namespace Code.Characters.Doods.AI
{
    // todo implement me
    public abstract class Decorator : BehaviorTreeNode
    {
        private BehaviorTreeNode _child;

        protected Decorator (Dood dood) : base(dood) { }

        protected override Status Update () { return Status.Failure; }
    }
}