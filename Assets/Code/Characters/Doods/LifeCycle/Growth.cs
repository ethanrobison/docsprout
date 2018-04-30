using System.Collections;
using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public class Growth : MonoBehaviour
    {
        private const float GROWTH_RATE = 20f;

        private DoodStatus _status;
        private DoodStage _stage;

        private ParticleSystem _particle;
        private AudioSource _pop;

        private void Start () {
            _particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _status = transform.Find("Status").gameObject.GetRequiredComponent<DoodStatus>();

            var plant = transform.Find("Dood/Body/Plant").gameObject;
            _pop = plant.GetRequiredComponent<AudioSource>();

            _stage = new DoodStage(this,
                new LifeCycleStage(BodyType.Cone, Maturity.Empty,
                    new LifeCycleStage(BodyType.Capsule, Maturity.Seedling,
                        new LifeCycleStage(BodyType.Capsule, Maturity.Sprout, null))));
        }

        private void Update () {
            var delta = (_status.Happiness - 50f) * GROWTH_RATE * Time.deltaTime * 0.01f;
            _stage.ChangeGrowth(delta);
        }


        public void StartTransition () { StartCoroutine(ChangePlant()); }

        private IEnumerator ChangePlant () {
            _particle.Play();
            yield return new WaitForSeconds(1f);
            _stage.GoToNextStage();
            _pop.Play();
        }
    }

    public class DoodStage
    {
        private const float GROW_AT = 100f;

        private readonly Growth _growth;
        private float _value;
        private LifeCycleStage _currentStage;
        private int _stepsLeft;

        private readonly GameObject _go;

        public DoodStage (Growth growth, LifeCycleStage stage) {
            _growth = growth;
            _go = _growth.gameObject;
            _currentStage = stage;
            ResetState();
        }

        public void ChangeGrowth (float delta) {
            _value = Mathf.Max(_value + delta, 0f);

            if (_value < GROW_AT) { return; }

            IncrementGrowth();
        }

        private void IncrementGrowth () {
            _value = 0f;
            if (_currentStage.Next == null) { return; }

            _stepsLeft--;
            if (_stepsLeft > 0) { return; }

            _growth.StartTransition();
        }

        private void ResetState () {
            _stepsLeft = (int) _currentStage.Maturity;

            var body = _go.transform.Find("Dood/Body").gameObject;
            body.GetComponent<MeshFilter>().mesh = _currentStage.Body;

            var plant = _go.transform.Find("Dood/Body/Plant").gameObject;
            var info = _currentStage.Leaf;
            plant.transform.localPosition = info.Offset;
            plant.GetComponent<MeshFilter>().mesh = info.Mesh;
        }

        public void GoToNextStage () {
            _currentStage = _currentStage.Next;
            ResetState();
        }
    }
}