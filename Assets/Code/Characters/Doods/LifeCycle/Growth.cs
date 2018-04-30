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

        public Maturity Stage {
            get { return _currentStage == null ? Maturity.Empty : _currentStage.Maturity; }
        }

        private readonly Growth _growth;
        private float _value;
        private LifeCycleStage _currentStage;


        private readonly GameObject _go;

        public DoodStage (Growth growth, LifeCycleStage stage) {
            _growth = growth;
            _go = _growth.gameObject;
            _currentStage = stage;
            ResetMeshes();
        }

        public void ChangeGrowth (float delta) {
            _value = Mathf.Max(_value + delta, 0f);

            if (_value < GROW_AT) { return; }

            if (_currentStage.Next == null) { return; }

            _growth.StartTransition();
            _value = 0f;
        }

        public void GoToNextStage () {
            _currentStage = _currentStage.Next;
            ResetMeshes();
        }

        private void ResetMeshes () {
            var body = _go.transform.Find("Dood/Body").gameObject;
            body.GetComponent<MeshFilter>().mesh = _currentStage.Body;

            var plant = _go.transform.Find("Dood/Body/Plant").gameObject;
            plant.GetComponent<MeshFilter>().mesh = _currentStage.Leaf;
        }
    }
}