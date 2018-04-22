using System;
using Code.Interaction;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public class DoodStatus : MonoBehaviour, IApproachable
    {
        private const float MAX_HAPPINESS = 100f;
        private const float MAGNITUDE = 5f;

        [Range(0f, 100f)] public float Happiness;

        private DoodColor _doodColor;
        private Waterable _waterable;
        private StatusDisplay _display;
        private bool _displaying;

        private void Start () {
            var dood = gameObject.GetRequiredComponentInParent<Dood>();
            _doodColor = dood.Comps.Color;
            _waterable = gameObject.GetRequiredComponentInParent<Waterable>();
            _display = new StatusDisplay(gameObject);
        }

        private float CalculateHappiness () {
            var delta = (_waterable.Status == 0 ? 5f : -1f) * MAGNITUDE * Time.deltaTime;
            return Mathf.Clamp(Happiness + delta, 0f, MAX_HAPPINESS);
        }

        private void Update () {
            Happiness = CalculateHappiness();
            _doodColor.Happiness = Happiness / MAX_HAPPINESS;

            if (_displaying) {
                _display.Show(_waterable.Status);
            }
            else {
                _display.Hide();
            }
        }

        // todo if you stand near a dood, their status display won't change
        public void OnApproach () { _displaying = true; }
        public void OnDepart () { _displaying = false; }
    }


    public class StatusDisplay
    {
        private readonly GameObject _needMore;
        private readonly GameObject _needLess;

        public StatusDisplay (GameObject go) {
            _needMore = go.transform.Find("NeedWater").gameObject;
            _needLess = go.transform.Find("StopWater").gameObject;
        }

        public void Show (int status) {
            // lol this could be a bit manipulation // but m'clarity
            switch (status) {
                case 0:
                    SetDisplays(false, false);
                    break;
                case 1:
                    SetDisplays(false, true);
                    break;
                case -1:
                    SetDisplays(true, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Hide () { SetDisplays(false, false); }

        private void SetDisplays (bool under, bool over) {
            _needMore.SetActive(under);
            _needLess.SetActive(over);
        }
    }
}