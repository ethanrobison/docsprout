using System.Collections.Generic;
using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public class DoodSpecies
    {
        public readonly Species Species;
        private readonly BodyType _body;
        private readonly Maturity _stageAfterHarvest;

        private Dictionary<Maturity, LifeCycleStage> _stages = new Dictionary<Maturity, LifeCycleStage>();

        public DoodSpecies (Species species, BodyType body) {
            Species = species;
            _body = body;
        }

        public DoodSpecies (Species species, BodyType body, Maturity stageAfterHarvest, SpeciesLifeCycles cycles) {
            Species = species;
            _body = body;
            _stageAfterHarvest = stageAfterHarvest;
            foreach (var cycle in cycles.LifeCycles) {
                AddLifeCycle(cycle.Maturity, cycle.Cycle);
            }
        }

        private void AddLifeCycle (Maturity maturity, LifeCycleStage cycle) {
            if (_stages.ContainsKey(maturity)) {
                _stages[maturity] = cycle;
                return;
            }

            _stages.Add(maturity, cycle);
        }

        public int GetGrowthStageCount (Maturity maturity) {
            LifeCycleStage cycle;
            if (_stages.TryGetValue(maturity, out cycle)) { return cycle.Values.GrowthStages; }

            Logging.Error("Species does not contain maturity " + maturity);
            return -1;
        }

        public Maturity GetNextStage (Maturity current) {
            LifeCycleStage cycle;
            if (_stages.TryGetValue(current, out cycle)) { return cycle.Values.Next; }

            Logging.Error("Species does not contain maturity " + current);
            return Maturity.Invalid;
        }

        public Maturity GetStageAfterHarvest () { return _stageAfterHarvest; }

        public Mesh GetBody () { return Doodopedia.GetBodyOfType(_body); }

        public MeshInfo GetLeaf (Maturity maturity) {
            LifeCycleStage cycle;
            if (_stages.TryGetValue(maturity, out cycle)) {
                return Doodopedia.GetLeafForBody(cycle.Values.LeafType, _body);
            }

            Logging.Error("Species does not contain maturity " + maturity);
            return Doodopedia.GetLeafForBody(LeafType.Seed, _body); // todo throw an exception? this is more graceful
        }

        public bool GetNeedOfType (Maturity maturity, NeedType type) { return _stages[maturity].GetNeedOfType(type); }

        public bool IsHarvestable (Maturity maturity) {
            LifeCycleStage cycle;
            if (_stages.TryGetValue(maturity, out cycle)) { return cycle.Values.Harvestable; }

            Logging.Error("Species does not contain maturity " + maturity);
            return false;
        }
    }

    [System.Serializable]
    public struct SpeciesLifeCycles
    {
        public List<MaturityLifeCyclePair> LifeCycles;
    }

    [System.Serializable]
    public struct MaturityLifeCyclePair
    {
        public Maturity Maturity;
        public LifeCycleStage Cycle;

        public MaturityLifeCyclePair (Maturity maturity, LifeCycleStage cycle) {
            Maturity = maturity;
            Cycle = cycle;
        }
    }
}