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
        private MeshFilter _filter;

        private void Start () {
            _particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _status = transform.Find("Status").gameObject.GetRequiredComponent<DoodStatus>();

            var plant = transform.Find("Dood/Capsule Dood/Plant").gameObject;
            _filter = plant.GetRequiredComponent<MeshFilter>();
            _pop = plant.GetRequiredComponent<AudioSource>();

            _stage = new DoodStage(this);
            _stage.AddSeed(new Seed(Next, new Seedling(Next, null)));
        }

        private void Update () {
            var delta = (_status.Happiness - 50f) * GROWTH_RATE * Time.deltaTime * 0.01f;
            _stage.ChangeGrowth(delta);
        }

        public void PlayEffectAndChange (Mesh plant) { StartCoroutine(ChangePlant(plant)); }

        private IEnumerator ChangePlant (Mesh plant) {
            _particle.Play();
            yield return new WaitForSeconds(1f);
            _filter.mesh = plant;
            _pop.Play();
        }
    }

    public class DoodStage
    {
        private const float GROW_AT = 100f;

        public Stage Stage {
            get { return _currentStage == null ? Stage.Empty : _currentStage.Stage; }
        }

        private readonly Growth _growth;
        private float _value;
        private LifeCyleStage _currentStage;


        private GameObject _go;

        public DoodStage (Growth growth) { _growth = growth; }

        public void AddSeed (Seed stage) { _currentStage = stage; }

        public void ChangeGrowth (float delta) {
            _value = Mathf.Max(_value + delta, 0f);

            if (_value < GROW_AT) { return; }

            if (_currentStage.Next == null) { return; }

            GoToNextStage();
            _value = 0f;
        }

        private void GoToNextStage () {
            if (_currentStage.Next == null) { return; }

            _currentStage = _currentStage.Next;
            _growth.PlayEffectAndChange(_currentStage.Mesh);
        }
    }
}