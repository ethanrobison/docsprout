using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public class WaterAdvertiser : Advertiser
    {
        public override void InteractWith (IAdvertisable user) {
            var mono = user as MonoBehaviour;
            if (mono == null) { return; }

            // todo soft-coded for doods
            var waterable = mono.GetComponentInParent<Waterable>();
            if (waterable != null) { waterable.Satisfy(); }
        }

        public override NeedType Satisfies () { return NeedType.Water; }
    }
}