using System.Collections.Generic;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public abstract class Advertiser : MonoBehaviour
    {
        private void OnTriggerEnter (Collider other) {
            var advertisable = other.GetComponent<IAdvertisable>();
            if (advertisable != null) { advertisable.AdvertiseTo(this); }
        }

        public abstract List<Need> GetNeedsSatisfied ();
    }

    public interface IAdvertisable
    {
        void AdvertiseTo (Advertiser advertiser);
    }
}