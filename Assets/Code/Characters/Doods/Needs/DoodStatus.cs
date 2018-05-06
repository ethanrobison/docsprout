using System.Collections.Generic;
using System.Linq;
using Code.Environment;
using Code.Environment.Advertising;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public class DoodStatus : MonoBehaviour, IApproachable, IAdvertisable, ISatisfiable
    {
        private const float MAX_HAPPINESS = 100f, MAGNITUDE = 5f;
        [Range(0f, 100f)] public float Happiness;
        public SmallSet Advertised { get; private set; }
        public SmallSet Satisfiable { get; private set; }

        private Dood _dood;
        private StatusDisplay _display;

        private bool _displaying;

        private readonly List<Advertiser> _advertisers = new List<Advertiser>();
        private readonly List<Satisfier> _satisfiers = new List<Satisfier>();

        public Need[] Needs { get; private set; }

        private void Start () {
            Advertised = new SmallSet();
            Satisfiable = new SmallSet();

            _dood = gameObject.GetRequiredComponentInParent<Dood>();
            Needs = GetComponents<Need>();
            _display = new StatusDisplay(gameObject);
        }

        private void Update () {
            var total = 0f;
            int numNeeds = 0;
            for (int i = 0, c = Needs.Length; i < c; i++) {
                if (Needs[i].IsEnabled()) numNeeds++;
                total += CalculateHappiness(Needs[i]);
            }

            total = Mathf.Clamp(total / numNeeds, -2 * MAGNITUDE, 2 * MAGNITUDE);

            Happiness = Mathf.Clamp(Happiness + total, 0f, MAX_HAPPINESS);
            _dood.Comps.Color.Happiness = Happiness / MAX_HAPPINESS;
        }

        private float CalculateHappiness (Need need) {
            _display.SetIconOfType(need.Type, need.Status);
            if (!need.IsEnabled()) {
                return 0f;
            }

            return (need.Status == 0 ? 3f : -2f) * MAGNITUDE * Time.deltaTime;
        }


        //
        // interfaces

        public Advertiser GetAdvertiserOfType (NeedType type) { return _advertisers.First(a => a.Satisfies() == type); }
        public Satisfier GetSatisfierOfType (NeedType type) { return _satisfiers.First(a => a.Satisfies() == type); }

        public Need GetNeedOfType (NeedType type) { return Needs.FirstOrDefault(need => need.Type == type); }

        public void OnApproach () { _dood.Comps.Color.IsInteracted = true; }
        public void OnDepart () { _dood.Comps.Color.IsInteracted = false; }
        public void Interact () { Game.Ctx.Doods.Doodex.Show(_dood); }

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