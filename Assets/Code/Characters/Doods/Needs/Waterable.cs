using Code.Environment.Advertising;
using Code.Utils;

namespace Code.Characters.Doods.Needs
{
    public class Waterable : Need, IAdvertisable
    {
        public void AdvertiseTo (Advertiser advertiser) { Logging.Log(advertiser.ToString()); }
        public void StopAdvertising (Advertiser advertiser) { }
    }
}