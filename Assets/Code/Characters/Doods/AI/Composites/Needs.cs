using System.Linq;
using Code.Characters.Doods.Needs;
using Code.Environment.Advertising;

namespace Code.Characters.Doods.AI
{
    public class NeedLevel : FilterSelector
    {
        private readonly Need _need;

        public NeedLevel (Dood dood, Need need) : base(dood) { _need = need; }
        protected override bool Precondition () { return _need.Status < 0; }
    }


    public class AdvertiserNear : FilterSelector
    {
        private readonly ushort _type;

        public AdvertiserNear (Dood dood, NeedType type) : base(dood) { _type = (ushort) type; }

        protected override bool Precondition () { return Dood.Comps.Status.Advertised.Contains(_type); }
    }


    public class NeedSatisfiable : FilterSelector
    {
        private readonly ushort _type;
        public NeedSatisfiable (Dood dood, NeedType type) : base(dood) { _type = (ushort) type; }

        protected override bool Precondition () { return Dood.Comps.Status.Satisfiable.Contains(_type); }
    }


    public class InteractWithSatisfier : BehaviorTreeNode
    {
        private readonly NeedType _type;
        private Satisfier _satisfier;

        public InteractWithSatisfier (Dood dood, NeedType type) : base(dood) { _type = type; }

        public override void OnInitialize () {
            base.OnInitialize();
            _satisfier = Dood.Comps.Status.GetSatisfierOfType(_type);
        }

        protected override Status Update () {
            if (_satisfier == null) { return Status.Failure; }

            _satisfier.InteractWith(Dood);
            return Status.Success;
        }
    }


    public class TreatsNear : FilterSelector
    {
        public TreatsNear (Dood dood) : base(dood) { }

        // todo this is not performant, but alas
        protected override bool Precondition () { return Dood.Comps.Status.Advertisers.Any(a => a.IsTreat); }
    }
}