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
        public float GrowthValue = 0f;
        public Mesh Next;
        private DoodStage _next;
        public ParticleSystem GrowthParticles;


        private DoodStatus _status;

        private MeshFilter _filter;

        // Use this for initialization
        void Start () {
            _status = gameObject.GetRequiredComponent<DoodStatus>();
            _filter = transform.FindChild("Plant").gameObject.GetRequiredComponent<MeshFilter>();

            _next = new DoodStage();
            _next.PlantMesh = Next;
            _next.NextStage = null;
        }

        // Update is called once per frame
        void Update () {
            if (_next == null) return;
//            GrowthValue += Mathf.Max(0f, _status.Happiness - 50f) * GrowthRate * Time.deltaTime;
            GrowthValue += GrowthRate * Time.deltaTime;
            if (GrowthValue >= GrowAt) {
                GrowthValue = 0f;
                StartCoroutine(ChangePlant(_next.PlantMesh));
                _next = _next.NextStage;
            }
        }

        private IEnumerator ChangePlant (Mesh plant) {
            GrowthParticles.Play();
            yield return new WaitForSeconds(1f);
            _filter.mesh = plant;
        }
    }

//    [Serializable]
    public class DoodStage
    {
        public DoodStage NextStage;
        public Mesh PlantMesh;
    }
}