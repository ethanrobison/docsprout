using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public class DoodStatus : MonoBehaviour
    {
        public float Happiness;
        public Waterable Waterable;

        private float _minHappiness;
        private float _maxHappiness;

        private float _waterMeter;
        private float[] _waterRange;

        private DoodColor _doodColor;

        private void Start () {
            GetComponent<Dood>();
            _doodColor = GetComponent<DoodColor>();

            _minHappiness = 0f;
            _maxHappiness = 100f;

            Waterable = GetComponent<Waterable>();
            _waterMeter = Waterable.StartingMeter;
            _waterRange = Waterable.NeedRange;
        }

        private void CalculateHappiness () {
            if (_waterRange[0] <= _waterMeter && _waterMeter <= _waterRange[1]) {
                Happiness += 5f * Time.deltaTime;
            }
            else {
                Happiness -= 5f * Time.deltaTime;
            }

            if (Happiness <= _minHappiness) {
                Happiness = _minHappiness;
            }
            else if (Happiness >= _maxHappiness) {
                Happiness = _maxHappiness;
            }
        }

        private void Update () {
            _waterMeter = Waterable.StartingMeter;
            CalculateHappiness();
            if (_doodColor) {
                _doodColor.Happiness = Happiness / _maxHappiness;
            }
        }
    }
}