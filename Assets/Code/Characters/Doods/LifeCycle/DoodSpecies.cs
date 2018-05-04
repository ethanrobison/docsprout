using System.Collections.Generic;
using Code.Characters.Doods.Needs;
using Code.Characters.Doods.LifeCycle;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    [System.Serializable]
    public class DoodSpeciesInfo
    {
        public readonly Doodopedia.DoodSpecies Species;

        private readonly Dictionary<Maturity, LifeCycleInfo> _lifeCycleInfo =
            new Dictionary<Maturity, LifeCycleInfo>();

        private readonly Maturity _stageAfterHarvest;
        private readonly BodyType _bodyType;

        public DoodSpeciesInfo (Doodopedia.DoodSpecies species, List<LifeCycleInfo> lifeCycleInfo,
            Maturity stageAfterHarvest, BodyType bodyType) {
            Species = species;

            foreach (var cycleInfo in lifeCycleInfo) {
                _lifeCycleInfo.Add(cycleInfo.Current, cycleInfo);
            }

            _stageAfterHarvest = stageAfterHarvest;
            _bodyType = bodyType;
        }
    }

    [System.Serializable]
    public struct LifeCycleInfo
    {
        public readonly Maturity Current;
        public readonly Maturity Next;
        public readonly MeshInfo Leaf;
        public readonly float GrowthRate;
    
        private readonly Dictionary<NeedType, NeedValuesInfo> _needValues;

        public LifeCycleInfo (Maturity current, Maturity next, MeshInfo leaf, float growthRate) {
            Current = current;
            Next = next;
            Leaf = leaf;
            GrowthRate = growthRate;
            _needValues = new Dictionary<NeedType, NeedValuesInfo>();
        }
    }

    [System.Serializable]
    public struct NeedValuesInfo
    {
        public readonly NeedType Type;
        public readonly float Bottom, Top, SatisfactionRate, DecayRate;

        public NeedValuesInfo (NeedType type, float bottom, float top, float satisfactionRate, float decayRate) {
            Type = type;
            Bottom = bottom;
            Top = top;
            SatisfactionRate = satisfactionRate;
            DecayRate = decayRate;
        }
    }

}