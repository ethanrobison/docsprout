using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public enum NeedType // Add new need types to the end
    {
        Water = 1,
        Sun = 2,
        Fun = 3,
    }

    public class Need : MonoBehaviour
    {
        private const float INCREASE = 20f, DECAY = 10f;
        private ParticleSystem _satisPart;

        public Vector3 Range;
        public NeedType Type;

        public int Status {
            get { return _values.Status; }
        }

        private NeedValues _values;


        private void Start () {
            _values = new NeedValues(Range.x, Range.z);
            _satisPart = transform.parent.Find("SatisfyingParticles").GetComponent<ParticleSystem>();
        }
        private void Update () { IncreaseNeed(Time.deltaTime); }

        public void Satisfy () {
            int PrevStatus = _values.Status;
            _values.ChangeValue(INCREASE);
            if (PrevStatus != 0 && _values.Status == 0) {
               _satisPart.Play();
            }
        }

        private void IncreaseNeed (float time) { _values.ChangeValue(-time * DECAY); }
    }

    public struct NeedValues
    {
        private const float MAX = 100f;
        private readonly float _bottom, _top;
        private float _value;

        public NeedValues (float bottom, float top) {
            _bottom = bottom;
            _top = top;
            _value = 50f;
        }

        public void ChangeValue (float delta) {
            var result = _value + delta;
            _value = Mathf.Clamp(result, 0f, MAX);
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