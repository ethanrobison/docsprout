using Code.Characters.Doods.Needs;
using Code.Utils;

namespace Code.Environment.Advertising
{
    public class WaterAdvertiser : Advertiser
    {
        public override void InteractWith (IAdvertisable user) { Logging.Log("Time to shower!"); }
        public override NeedType Satisfies () { return NeedType.Water; }
    }
}