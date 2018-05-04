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
        Flower,
        Fruit
    }

    public class LifeCycleStage
    {
        public Maturity Maturity { get; protected set; }
        public bool Harvestable;

        public LifeCycleStage Next { get; private set; } // linked list of growth stages
        // todo have a cycle in the last step to do harvesting, etc.

        public Mesh Body { get; private set; }
        public MeshInfo Leaf { get; private set; }

        public LifeCycleStage (BodyType body, Maturity maturity, LifeCycleStage next, bool harvestable = false) {
            Next = next;
            Body = Doodopedia.GetBodyOfType(body);
            Leaf = Doodopedia.GetLeafOfType(maturity);
            Maturity = maturity;
            Harvestable = harvestable;
        }
    }
}