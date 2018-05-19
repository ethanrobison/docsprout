using System;
using Code.Characters.Doods.LifeCycle;
using Code.Utils;

namespace Code.Characters.Player
{
    public class EconomyManager : IContextManager
    {
        public EconomyStats Stats { get; private set; }

        public void Initialize () {
            Stats = new EconomyStats();
            Stats.Initialize();
        }

        public void ShutDown () { }

        public void Harvest (Growth growth) {
            if (growth.Harvest()) { Stats.ChangeFroot(1); }
        }
    }

    public class EconomyStats : IContextManager
    {
        private int _frootCount;

        public Action<int> OnFrootChanged = n => { };

        public void Initialize () { }
        public void ShutDown () { }

        private bool CanSpendFroot (int count) { return _frootCount >= count; }


        public void ChangeFroot (int delta) {
            _frootCount += delta;
            OnFrootChanged.Invoke(_frootCount);
        }

        public void SpendFroot (int count) {
            if (!CanSpendFroot(count)) { return; }

            ChangeFroot(-count);
        }
    }
}