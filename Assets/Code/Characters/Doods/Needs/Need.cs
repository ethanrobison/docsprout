using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public abstract class Need : MonoBehaviour
    {
        public float StartingMeter = 50f;
        public float[] NeedRange; // range that the dood needs to be c:
        public float NeedIncr; // give need
        public float NeedDecr; // neglect need

        private NeedValues _values;

        protected void Start () { _values = new NeedValues {Max = 100f, Min = 0f, Meter = StartingMeter}; }

        public void IncrMeter () { _values.ChangeValue(NeedIncr); }

        private void DecrMeter (float time) { _values.ChangeValue(-time * NeedDecr); }

        protected void Update () { DecrMeter(Time.deltaTime); }

        private struct NeedValues
        {
            public float Min;
            public float Max;
            public float Meter;

            public void ChangeValue (float delta) {
                var result = Meter + delta;
                Meter = Mathf.Clamp(result, Min, Max);
            }
        }
    }
}