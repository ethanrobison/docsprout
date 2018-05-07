using System;
using System.Collections;
using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public class Growth : MonoBehaviour
    {
        public Action<DoodSpecies, Maturity> OnGrow;

        private const float GROWTH_RATE = 0.3f;

        public Species Species;

        private DoodStatus _status;
        private DoodStage _stage;

        private ParticleSystem _particle;
        private AudioSource _pop;


        private void Start () {
            _particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _status = transform.Find("Status").gameObject.GetRequiredComponent<DoodStatus>();

            var plant = transform.Find("Dood/Body/Plant").gameObject;
            _pop = plant.GetRequiredComponent<AudioSource>();

            _stage = new DoodStage(this, Species);
        }

        private void Update () {
            var delta = (_status.Happiness - 50f) * GROWTH_RATE * Time.deltaTime;
            _stage.ChangeGrowth(delta);
        }


        public void StartTransition () { StartCoroutine(ChangePlant()); }

        private IEnumerator ChangePlant () {
            _particle.Play();
            yield return new WaitForSeconds(1f);
            _stage.GoToNextStage();
            _pop.Play();
        }

        public void Harvest () { _stage.Harvest(); }
    }

    public class DoodStage
    {
        private const float GROW_AT = 100f;

        private readonly DoodSpecies _species;
        private readonly Growth _growth;
        private float _value;
        private Maturity _currentStage;
        private int _stepsLeft;

        private readonly GameObject _go;

        public DoodStage (Growth growth, Species species) {
            _growth = growth;
            _go = _growth.gameObject;
            _species = Doodopedia.GetDoodSpecies(species);
            _currentStage = Maturity.Seed;
            ResetState();
        }

        public void ChangeGrowth (float delta) {
            _value = Mathf.Max(_value + delta, 0f);

            if (_value < GROW_AT) { return; }

            IncrementGrowth();
        }

        private void IncrementGrowth () {
            _value = 0f;
            if (_species.GetNextStage(_currentStage) == Maturity.Empty) { return; }

            _stepsLeft--;
            if (_stepsLeft > 0) { return; }

            _growth.StartTransition();
        }

        private void ResetState () {
            _stepsLeft = _species.GetNumGrowthStages(_currentStage);

            var body = _go.transform.Find("Dood/Body").gameObject;
            body.GetComponent<MeshFilter>().mesh = _species.GetBody();

            var plant = _go.transform.Find("Dood/Body/Plant").gameObject;
            var info = _species.GetLeaf(_currentStage);
            plant.transform.localPosition = info.Offset;
            plant.GetComponent<MeshFilter>().mesh = info.Mesh;
            plant.GetComponent<Renderer>().material = info.Material;

            var froot = _go.transform.Find("Dood/Body/Plant/Froot");
            froot.GetComponent<Renderer>().enabled = _species.IsHarvestable(_currentStage);
        }

        public void GoToNextStage () {
            _currentStage = _species.GetNextStage(_currentStage);
            ResetState();
            _growth.OnGrow(_species, _currentStage);
        }

        public void Harvest () {
            if (!_species.IsHarvestable(_currentStage)) { return; }
            _value = 0f;
            _currentStage = _species.GetStageAfterHarvest();
            ResetState();
            _growth.OnGrow(_species, _currentStage);
        }
    }
}