using Code.Characters.Doods.Needs;
using Code.Environment.Advertising;

namespace Code.Characters.Doods.AI
{
    public class NeedLevel<TNeed> : FilterSelector where TNeed : Need
    {
        private readonly TNeed _need;

        public NeedLevel (Dood dood, TNeed need) : base(dood) { _need = need; }
        protected override bool Precondition () { return _need.Status < 0; }
    }

    public class AdvertiserNear : FilterSequence
    {
        private readonly ushort _type;

        public AdvertiserNear (Dood dood, NeedType type) : base(dood) { _type = (ushort) type; }

        protected override bool Precondition () { return Dood.Comps.Status.Advertised.Contains(_type); }
    }

    public class InteractWithAdvertiser : BehaviorTreeNode
    {
        private readonly NeedType _type;
        private Advertiser _advertiser;

        public InteractWithAdvertiser (Dood dood, NeedType type) : base(dood) { _type = type; }

        public override void OnInitialize () {
            base.OnInitialize();
            _advertiser = Dood.Comps.Status.GetAdvertiserOfType(_type);
        }

        protected override Status Update () {
            if (_advertiser == null) { return Status.Failure; }

            _advertiser.InteractWith(Dood.Comps.Status);
            return Status.Success;
        }
    }
}