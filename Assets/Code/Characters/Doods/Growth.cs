using System;
using System.Collections;
using Code.Characters.Doods.Needs;
using UnityEngine;
using Code.Utils;

namespace Code.Characters.Doods
{
    public class Growth : MonoBehaviour
    {
        public float GrowthRate = 10f;
        public float GrowAt = 100f;
        public float GrowthValue;
        public Mesh Next;
        public ParticleSystem GrowthParticles;

        private DoodStage _next;
        private DoodStatus _status;

        private AudioSource _pop;
        private MeshFilter _filter;

        private void Start () {
            _status = gameObject.GetRequiredComponentInChildren<DoodStatus>();
            var plant = transform.Find("Dood/Capsule Dood/Plant").gameObject;
            _filter = plant.GetRequiredComponent<MeshFilter>();
            _pop = plant.GetRequiredComponent<AudioSource>();

            _next = new DoodStage {
                PlantMesh = Next,
                NextStage = null
            };
        }

        private void Update () {
            if (_next == null) { enabled = false; } // turn me off

            GrowthValue += Mathf.Max(0f, (_status.Happiness - 50f) / 100f) * GrowthRate * Time.deltaTime;

            if (GrowthValue < GrowAt) { return; }

            GrowthValue = 0f;
            StartCoroutine(ChangePlant(_next.PlantMesh));
            _next = _next.NextStage;
        }

        private IEnumerator ChangePlant (Mesh plant) {
            GrowthParticles.Play();
            yield return new WaitForSeconds(1f);
            _filter.mesh = plant;
            _pop.Play();
        }
    }

    public class DoodStage
    {
        public DoodStage NextStage;
        public Mesh PlantMesh;
    }
}