using Code.Characters.Doods;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Interaction
{
    public class GetDoodStatus : MonoBehaviour, IApproachable
    {
        public Dood Dood;

        public GameObject StatusDisplay;

        public GameObject NeedWater;
        public GameObject StopWater;

        private Waterable _waterable;

        void IApproachable.OnApproach () {
            StatusDisplay.SetActive(true);
            // display happiness meter
        }

        void IApproachable.OnDepart () { StatusDisplay.SetActive(false); }

        private void Start () { _waterable = Dood.GetComponent<Waterable>(); }

        private void Update () {
            if (!_waterable) { return; }

            if (_waterable.StartingMeter < _waterable.NeedRange[0]) {
                NeedWater.SetActive(true);
                StopWater.SetActive(false);
            }
            else if (_waterable.StartingMeter > _waterable.NeedRange[1]) {
                StopWater.SetActive(true);
                NeedWater.SetActive(false);
            }
            else {
                NeedWater.SetActive(false);
                StopWater.SetActive(false);
            }
        }
    }
}