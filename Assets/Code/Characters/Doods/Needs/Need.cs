using System.Collections.Generic;
using Code.Characters.Doods.LifeCycle;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public enum NeedType // Do not change numbers
    {
        Water = 1,
        Sun = 2,
        Fun = 3,
        Food = 4
    }

    public class Need : MonoBehaviour
    {
        private static readonly Dictionary<NeedType, NeedValues> NeedValues = new Dictionary<NeedType, NeedValues> {
            {NeedType.Water, new NeedValues(45f, 100f, 10f, 1f)},
            {NeedType.Sun, new NeedValues(40f, 100f, 7f, .6f)},
            {NeedType.Fun, new NeedValues(30f, 100f, 20f, 5f)},
            {NeedType.Food, new NeedValues(40f, 100f, 5f, .4f)}
        };

        public NeedType Type;

        public float Value {
            get { return _values.Value; }
        }

        private ParticleSystem _satisfactionParticle;
        private NeedValues _values;

        public int Status {
            get { return _values.Status; }
        }


        private void Start () {
            var growth = transform.parent.gameObject.GetRequiredComponent<Growth>();
            growth.OnGrow += OnGrow;
            var template = NeedValues[Type];
            _values = new NeedValues(template.Bottom, template.Top, template.SatisfactionRate, template.DecayRate) {
                Enabled = Doodopedia.GetDoodSpecies(growth.Species).GetNeedOfType(Maturity.Seed, Type)
            };

            _satisfactionParticle = transform.parent.Find("SatisfyingParticles").GetComponent<ParticleSystem>();
        }


        private void Update () { IncreaseNeed(Time.deltaTime); }

        public void Satisfy () {
            int PrevStatus = _values.Status;
            _values.SatisfyNeed(Time.deltaTime);
            if (PrevStatus != 0 && _values.Status == 0) {
                _satisfactionParticle.Play();
            }
        }

        private void IncreaseNeed (float time) { _values.IncreaseNeed(time); }

        private void OnGrow (DoodSpecies species, Maturity maturity) {
            _values.Enabled = species.GetNeedOfType(maturity, Type);
        }


        public bool IsEnabled () { return _values.Enabled; }
    }

    public class NeedValues
    {
        private const float MAX = 100f;
        public float Bottom, Top;
        public float SatisfactionRate, DecayRate;
        public float Value { get; private set; }
        private bool _enabled = true;

        public NeedValues (float bottom, float top, float satisfactionRate, float decayRate) {
            Bottom = bottom;
            Top = top;
            SatisfactionRate = satisfactionRate;
            DecayRate = decayRate;
            Value = 50f;
        }

        private void ChangeValue (float delta) {
            var result = Value + delta;
            Value = Mathf.Clamp(result, 0f, MAX);
        }

        public void SatisfyNeed (float time) { ChangeValue(time * SatisfactionRate); }

        public void IncreaseNeed (float time) { ChangeValue(-time * DecayRate); }

        public int Status {
            get {
                if (!_enabled) return 0;
                return Value < Bottom ? -1 :
                    Value > Top ? 1 :
                    0;
            }
        }

        public bool Enabled {
            get { return _enabled; }
            set {
                if (!_enabled && value) {
                    Value = 50f;
                }

                _enabled = value;
            }
        }
    }
}