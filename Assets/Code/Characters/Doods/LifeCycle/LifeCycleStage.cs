using System.Collections.Generic;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public enum Maturity
    {
        // todo something more graceful than this
        Empty = 0,
        Seed = 1,
        Seedling = 2,
        Sprout = 4,
        Bud = 8,
        Fullgrown
    }

    public class LifeCycleStage
    {
        public LifeCycleValues Values;

        private Dictionary<NeedType, int> _needs = new Dictionary<NeedType, int>();

        public LifeCycleStage (LifeCycleValues values, LifeCycleNeeds needs) {
            Values = values;
            foreach (var need in needs.Needs) {
                if (need.Value > 0) EnableNeed(need.Type);
                else {
                    DisableNeed(need.Type);
                }
            }
        }

        public LifeCycleStage (LifeCycleValues values) { Values = values; }

        public void EnableNeed (NeedType type) {
            if (_needs.ContainsKey(type)) {
                _needs[type] = 1;
                return;
            }

            _needs.Add(type, 1);
        }

        public void DisableNeed (NeedType type) {
            if (_needs.ContainsKey(type)) {
                _needs[type] = 0;
                return;
            }

            _needs.Add(type, 0);
        }
        
        public int GetNeedOfType(NeedType type) {
            return _needs[type];
        }
        
    }

    [System.Serializable]
    public struct LifeCycleValues
    {
        public Maturity Next;
        public int GrowthStages;
        public LeafType LeafType;
        public bool Harvestable;

        public LifeCycleValues (Maturity next, int growthStages, LeafType leafType,
            bool harvestable = false) {
            Next = next;
            GrowthStages = growthStages;
            LeafType = leafType;
            Harvestable = harvestable;
        }
    }

    [System.Serializable]
    public struct LifeCycleNeeds
    {
        public List<NeedTypeIntPair> Needs;
    }

    [System.Serializable]
    public struct NeedTypeIntPair
    {
        public NeedType Type;
        public int Value;

        public NeedTypeIntPair (NeedType type, int value) {
            Type = type;
            Value = value;
        }
    }
}