using System;
using System.Collections.Generic;
using Code.Characters.Doods.Needs;
using Code.Characters.Doods.LifeCycle;
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

        public int GetNumGrowthStages (Maturity maturity) {
            LifeCycleStage cycle;
            Logging.Assert(_stages.TryGetValue(maturity, out cycle),
                "Species does not contain maturity " + maturity.ToString());
            return cycle.Values.GrowthStages;
        }

        public Maturity GetNextStage (Maturity current) {
            LifeCycleStage cycle;
            Logging.Assert(_stages.TryGetValue(current, out cycle),
                "Species does not contain maturity " + current.ToString());
            return cycle.Values.Next;
        }

        public Maturity GetStageAfterHarvest () { return _stageAfterHarvest; }

        public Mesh GetBody () { return Doodopedia.GetBodyOfType(_body); }

        public MeshInfo GetLeaf (Maturity maturity) {
            LifeCycleStage cycle;
            Logging.Assert(_stages.TryGetValue(maturity, out cycle),
                "Species does not contain maturity " + maturity.ToString());

            return Doodopedia.GetLeafForBody(cycle.Values.LeafType, _body);
        }

        public int GetNeedOfType (Maturity maturity, NeedType type) { return _stages[maturity].GetNeedOfType(type); }

        public bool IsHarvestable (Maturity maturity) {
            LifeCycleStage cycle;
            Logging.Assert(_stages.TryGetValue(maturity, out cycle),
                "Species does not contain maturity " + maturity.ToString());

            return cycle.Values.Harvestable;
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