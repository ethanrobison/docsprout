using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public enum NeedType // Do not change numbers
    {
        Water = 1,
        Sun = 2,
        Fun = 3,
    }

    public class Need : MonoBehaviour
    {
        public float SatisfactionRate, DecayRate;

        public Vector3 Range;
        public NeedType Type;

        public int Status {
            get { return _values.Status; }
        }

        private NeedValues _values;


        private void Start () { _values = new NeedValues(Range.x, Range.z, SatisfactionRate, DecayRate); }
        private void Update () { IncreaseNeed(Time.deltaTime); }

        public void Satisfy () { _values.SatisfyNeed(Time.deltaTime); }

        private void IncreaseNeed (float time) { _values.IncreaseNeed(time); }
        
        public void UpdateValues(float bottom, float top, float satisfactionRate, float decayRate) {
            _values.UpdateValues(bottom, top, satisfactionRate, decayRate);
        }
        
        
    }

    public struct NeedValues
    {
        private const float MAX = 100f;
        private float _bottom, _top;
        private float _satisfactionRate, _decayRate;
        private float _value;

        public NeedValues (float bottom, float top, float satisfactionRate, float decayRate) {
            _bottom = bottom;
            _top = top;
            _satisfactionRate = satisfactionRate;
            _decayRate = decayRate;
            _value = 50f;
        }

        private void ChangeValue (float delta) {
            var result = _value + delta;
            _value = Mathf.Clamp(result, 0f, MAX);
        }
        
        public void SatisfyNeed(float time) {
            ChangeValue(time * _satisfactionRate);
        }
        
        public void IncreaseNeed(float time) {
            ChangeValue(-time * _decayRate);
        }
        
        public void UpdateValues(float bottom, float top, float satisfactionRate, float decayRate) {
            _bottom = bottom;
            _top = top;
            _satisfactionRate = satisfactionRate;
            _decayRate = decayRate;
        }
        

        public int Status {
            get {
                return _value < _bottom ? -1 :
                    _value > _top ? 1 :
                    0;
            }
        }
    }
}