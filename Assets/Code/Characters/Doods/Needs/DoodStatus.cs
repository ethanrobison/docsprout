using System.Collections.Generic;
using System.Linq;
using Code.Environment;
using Code.Environment.Advertising;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public enum NeedType
    {
        None,
        Water,

//        Sun,
//        Food,
        Fun,
    }


    public class DoodStatus : MonoBehaviour, IApproachable, IAdvertisable, ISatisfiable
    {
        private const float MAX_HAPPINESS = 100f, MAGNITUDE = 5f;
        [Range(0f, 100f)] public float Happiness;
        public SmallSet Advertised { get; private set; }
        public SmallSet Satisfiable { get; private set; }

        private Dood _dood;
        private Need _waterable;
        private StatusDisplay _display;

        private bool _displaying;

        private readonly List<Advertiser> _advertisers = new List<Advertiser>();
        private readonly List<Satisfier> _satisfiers = new List<Satisfier>();

        private Need[] _needs;

        private void Start () {
            Advertised = new SmallSet();
            Satisfiable = new SmallSet();

            _dood = gameObject.GetRequiredComponentInParent<Dood>();
            _needs = GetComponents<Need>();
            _waterable = GetNeedOfType(NeedType.Water);
            _display = new StatusDisplay(gameObject);
        }

        private void Update () {
            Happiness = CalculateHappiness();
            _dood.Comps.Color.Happiness = Happiness / MAX_HAPPINESS;

            _display.Show(_waterable.Status);
        }

        private float CalculateHappiness () {
            var delta = (_waterable.Status == 0 ? 5f : -1f) * MAGNITUDE * Time.deltaTime;
            return Mathf.Clamp(Happiness + delta, 0f, MAX_HAPPINESS);
        }

        //
        // interfaces

        public Advertiser GetAdvertiserOfType (NeedType type) { return _advertisers.First(a => a.Satisfies() == type); }
        public Satisfier GetSatisfierOfType (NeedType type) { return _satisfiers.First(a => a.Satisfies() == type); }

        public Need GetNeedOfType (NeedType type) { return _needs.FirstOrDefault(need => need.Type == type); }

        public void OnApproach () { _dood.Comps.Color.IsInteracted = true; }
        public void OnDepart () { _dood.Comps.Color.IsInteracted = false; }
        public void Interact () { }

        public void AdvertiseTo (Advertiser advertiser) {
            _advertisers.Add(advertiser);
            Advertised.Add((ushort) advertiser.Satisfies());
        }

        public void StopAdvertising (Advertiser advertiser) {
            var type = advertiser.Satisfies();
            var success = _advertisers.Remove(advertiser);
            Logging.Assert(success, "Tried to remove advertiser from list but was missing.");

            var c = _advertisers.Count(a => a.Satisfies() == type);
            if (c == 0) { Advertised.Remove((ushort) type); }
        }

        public void AllowSatisfaction (Satisfier satisfier) {
            _satisfiers.Add(satisfier);
            Satisfiable.Add((ushort) satisfier.Satisfies());
        }

        public void ForbidSatisfaction (Satisfier satisfier) {
            var type = satisfier.Satisfies();
            var success = _satisfiers.Remove(satisfier);
            Logging.Assert(success, "Tried to remove advertiser from list but was missing.");

            var c = _satisfiers.Count(a => a.Satisfies() == type);
            if (c == 0) { Satisfiable.Remove((ushort) type); }
        }
    }
}