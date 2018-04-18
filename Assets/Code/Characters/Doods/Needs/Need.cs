using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public abstract class Need : MonoBehaviour
    {
        public float StartingMeter = 50f;
        public Vector3 Range;
        private const float INCREASE = 40f;

        private const float DECAY = 10f;
//        public float NeedIncr; // give need
//        public float NeedDecr; // neglect need

        public int Status {
            get { return _values.Status; }
        }

        private NeedValues _values;


        protected void Start () { _values = new NeedValues(Range.x, Range.y, Range.z); }

        public void IncrMeter () { _values.ChangeValue(INCREASE); }

        private void DecrMeter (float time) { _values.ChangeValue(-time * DECAY); }

        protected void Update () { DecrMeter(Time.deltaTime); }
    }

    public struct NeedValues
    {
        private const float MAX = 100f;
        private readonly float _bottom, _center, _top;
        public float Value;

        public NeedValues (float bottom, float center, float top) {
            _bottom = bottom;
            _center = center;
            _top = top;
            Value = 50f;
        }

        public void ChangeValue (float delta) {
            var result = Value + delta;
            Value = Mathf.Clamp(result, 0f, MAX);
        }

        public int Status {
            get {
                return
                    Value < _bottom ? -1 :
                    Value > _top ? 1 :
                    0;
            }
        }
    }
}