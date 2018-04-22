using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public abstract class Advertiser : MonoBehaviour
    {
        private void OnTriggerEnter (Collider other) {
            var advertisable = other.GetComponentInChildren<IAdvertisable>();
            if (advertisable != null) { advertisable.AdvertiseTo(this); }
        }

        private void OnTriggerExit (Collider other) {
            var advertisable = other.GetComponentInChildren<IAdvertisable>();
            if (advertisable != null) { advertisable.StopAdvertising(this); }
        }

        public abstract void InteractWith (IAdvertisable user);
        public abstract NeedType Satisfies ();
    }

    public interface IAdvertisable
    {
        void AdvertiseTo (Advertiser advertiser);
        void StopAdvertising (Advertiser advertiser);
    }
}