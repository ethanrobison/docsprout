using Code.Characters.Doods;
using Code.Characters.Doods.Needs;

namespace Code.Environment.Advertising
{
    public class WaterSatisfier : Satisfier
    {
        public override void InteractWith (Dood dood) { dood.Comps.Status.GetNeedOfType(Satisfies()).Satisfy(); }
        public override NeedType Satisfies () { return NeedType.Water; }
    }
}