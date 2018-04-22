using Code.Characters.Doods.Needs;

namespace Code.Characters.Doods.AI
{
    public class GoToAdvertiser<TNeed> : BehaviorTreeNode where TNeed : Need
    {
        public GoToAdvertiser (Dood dood) : base(dood) { }
        protected override Status Update () { return Status.Failure; }
    }
}