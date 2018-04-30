using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public enum Maturity
    {
        Empty,
        Seed,
        Seedling,
        Sprout,
        Bud,
        Flower,
        Fruit
    }

    public class LifeCycleStage
    {
        public Maturity Maturity { get; protected set; }

        public LifeCycleStage Next { get; private set; } // linked list of growth stages
        // todo have a cycle in the last step to do harvesting, etc.

        public Mesh Body { get; private set; }
        public Mesh Leaf { get; private set; }

        public LifeCycleStage (BodyType body, Maturity maturity, LifeCycleStage next) {
            Next = next;
            Body = Doodopedia.GetBodyOfType(body);
            Leaf = Doodopedia.GetLeafOfType(maturity);
            Maturity = maturity;
        }
    }
}