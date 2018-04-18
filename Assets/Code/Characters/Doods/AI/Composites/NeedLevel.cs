using Code.Characters.Doods.Needs;
using Code.Doods.AI;

namespace Code.Characters.Doods.AI
{
    public class NeedLevel<TNeed> : FilterSequence where TNeed : Need
    {
        private readonly TNeed _need;

        public NeedLevel (Dood dood, TNeed need) : base(dood) { _need = need; }
        protected override bool Precondition () { return _need.Status < 0; }
    }
}