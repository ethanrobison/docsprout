using Code.Characters.Doods.LifeCycle;

namespace Code.Characters.Player
{
    public class EconomyManager : IContextManager
    {
        public void Initialize () { }
        public void ShutDown () { }

        public void HarvestGrowth (Growth growth) { growth.Harvest(); }
    }
}