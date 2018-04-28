using Code.Characters.Doods;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public class Advertiser : MonoBehaviour
    {
        private void OnTriggerEnter (Collider other) {
            var advertisable = other.GetComponentInChildren<IAdvertisable>();
            if (advertisable != null) { advertisable.AdvertiseTo(this); }
        }

        private void OnTriggerExit (Collider other) {
            var advertisable = other.GetComponentInChildren<IAdvertisable>();
            if (advertisable != null) { advertisable.StopAdvertising(this); }
        }
        
        [SerializeField] private NeedType _satisfies;
        
        public NeedType Satisfies() {
            return _satisfies;
        }
    }

    public interface IAdvertisable
    {
        void AdvertiseTo (Advertiser advertiser);
        void StopAdvertising (Advertiser advertiser);
    }
}