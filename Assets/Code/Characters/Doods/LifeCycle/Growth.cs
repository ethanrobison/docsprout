using System.Collections;
using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public class Growth : MonoBehaviour
    {
        private const float GROWTH_RATE = 10f;
        public Mesh Next;

        private DoodStatus _status;
        private DoodStage _stage;

        private ParticleSystem _particle;
        private AudioSource _pop;

        private void Start () {
            _particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _status = transform.Find("Status").gameObject.GetRequiredComponent<DoodStatus>();

            var plant = transform.Find("Dood/Body/Plant").gameObject;
            _pop = plant.GetRequiredComponent<AudioSource>();

            _stage = new DoodStage(this);
            _stage.AddSeed(
                new LifeCycleStage(null, Maturity.Seedling,
                    new LifeCycleStage(null, Maturity.Sprout, null)));
        }

        private void Update () {
            var delta = (_status.Happiness - 50f) * GROWTH_RATE * Time.deltaTime * 0.01f;
            _stage.ChangeGrowth(delta);
        }

        public void PlayEffectAndChange (LifeCycleStage stage) { StartCoroutine(ChangePlant(stage.Leaf)); }

        private IEnumerator ChangePlant (Mesh plant) {
            _particle.Play();
            yield return new WaitForSeconds(1f);
            SetLeafMesh(plant);
            _pop.Play();
        }

        private void SetBodyMesh (Mesh body) { }

        private void SetLeafMesh (Mesh leaf) {
            var plant = transform.Find("Dood/Body/Plant").gameObject;
            plant.GetComponent<MeshFilter>().mesh = leaf;
        }
    }

    public class DoodStage
    {
        private const float GROW_AT = 100f;

        public Maturity Stage {
            get { return _currentStage == null ? Maturity.Empty : _currentStage.Maturity; }
        }

        private readonly Growth _growth;
        private float _value;
        private LifeCycleStage _currentStage;


        private GameObject _go;

        public DoodStage (Growth growth) { _growth = growth; }

        public void AddSeed (LifeCycleStage stage) { _currentStage = stage; }

        public void ChangeGrowth (float delta) {
            _value = Mathf.Max(_value + delta, 0f);

            if (_value < GROW_AT) { return; }

            if (_currentStage.Next == null) { return; }

            GoToNextStage();
            _value = 0f;
        }

        private void GoToNextStage () {
            _currentStage = _currentStage.Next;
            _growth.PlayEffectAndChange(_currentStage);
        }
    }
}