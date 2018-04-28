using Code.Characters.Doods.Needs;

namespace Code.Characters.Doods.AI
{
    public class GoToAdvertiser : BehaviorTreeNode
    {
        private readonly ushort _type;
        public GoToAdvertiser (Dood dood, NeedType type) : base(dood) { _type = (ushort) type; }

        public override void OnTerminate (Status result) {
            base.OnTerminate(result);
            Dood.StopMoving();
        }

        protected override Status Update () {
            var goal = Dood.Comps.Status.GetAdvertiserOfType((NeedType) _type);
            return Dood.MoveTowards(goal.transform.position, 0.1f) ? Status.Success : Status.Running;
        }
    }
}