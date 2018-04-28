using System;
using System.Collections.Generic;
using System.Linq;
using Code.Characters.Player.Interaction;
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
//        Fun,
    }


    public class DoodStatus : MonoBehaviour, IApproachable, IAdvertisable
    {
        private const float MAX_HAPPINESS = 100f, MAGNITUDE = 5f;
        [Range(0f, 100f)] public float Happiness;

        private Dood _dood;
        private Waterable _waterable;
        private StatusDisplay _display;

        private bool _displaying;

        public SmallSet Advertised = new SmallSet();
        private readonly List<Advertiser> _advertisers = new List<Advertiser>();

        private void Start () {
            _dood = gameObject.GetRequiredComponentInParent<Dood>();
            _waterable = gameObject.GetRequiredComponentInParent<Waterable>();
            _display = new StatusDisplay(gameObject);
        }

        private void Update () {
            Happiness = CalculateHappiness();
            _dood.Comps.Color.Happiness = Happiness / MAX_HAPPINESS;

//            if (_displaying) {
            _display.Show(_waterable.Status);
//            }
//            else {
//                _display.Hide();
//            }
        }

        private float CalculateHappiness () {
            var delta = (_waterable.Status == 0 ? 5f : -1f) * MAGNITUDE * Time.deltaTime;
            return Mathf.Clamp(Happiness + delta, 0f, MAX_HAPPINESS);
        }

        //
        // interfaces

        public Advertiser GetAdvertiserOfType (NeedType type) { return _advertisers.First(a => a.Satisfies() == type); }

        public void OnApproach () { }
        public void OnDepart () { }

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
    }
}