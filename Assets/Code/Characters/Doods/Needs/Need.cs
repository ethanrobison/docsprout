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

        public Vector3 Range;
        public NeedType Type;

        public int Status {
            get { return _values.Status; }
        }

        public float Value {
            get { return _values.Value; }
        }

        private ParticleSystem _satisfactionParticle;
        private NeedValues _values;


        private void Start () {
            _values = new NeedValues(Range.x, Range.z);
            _satisfactionParticle = transform.parent.Find("SatisfyingParticles").GetComponent<ParticleSystem>();
        }

        private void Update () { IncreaseNeed(Time.deltaTime); }

        public void Satisfy () {
            var prevStatus = _values.Status;
            _values.ChangeValue(INCREASE);
            if (prevStatus != 0 && _values.Status == 0) {
                _satisfactionParticle.Play();
            }
        }

        private void IncreaseNeed (float time) { _values.ChangeValue(-time * DECAY); }
    }

    public class NeedValues
    {
        private const float MAX = 100f;
        private readonly float _bottom, _top;
        public float Value { get; private set; }

        public NeedValues (float bottom, float top) {
            _bottom = bottom;
            _top = top;
            Value = 50f;
        }

        public void ChangeValue (float delta) {
            var result = Value + delta;
            Value = Mathf.Clamp(result, 0f, MAX);
        }

        public int Status {
            get {
                return Value < _bottom ? -1 :
                    Value > _top ? 1 :
                    0;
            }
        }
    }
}