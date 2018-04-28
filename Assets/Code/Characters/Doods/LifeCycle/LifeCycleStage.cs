using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public enum Stage
    {
        Empty,
        Seed,
        Seedling,
        Sprout,
        Bud,
        Flower,
        Fruit
    }

    public abstract class LifeCyleStage
    {
        public Stage Stage { get; protected set; }

        public LifeCyleStage Next { get; private set; } // linked list of growth stages
        // todo have a cycle in the last step to do harvesting, etc.

        public Mesh Mesh { get; private set; }

        protected LifeCyleStage (Mesh mesh, LifeCyleStage next) {
            Next = next;
            Mesh = mesh;
        }
    }

    public class Seed : LifeCyleStage
    {
        public Seed (Mesh mesh, LifeCyleStage next) : base(mesh, next) { Stage = Stage.Seed; }
    }

    public class Seedling : LifeCyleStage
    {
        public Seedling (Mesh mesh, LifeCyleStage next) : base(mesh, next) { Stage = Stage.Seedling; }
    }

    public class Sprout : LifeCyleStage
    {
        public Sprout (Mesh mesh, LifeCyleStage next) : base(mesh, next) { Stage = Stage.Sprout; }
    }

    public class Bud : LifeCyleStage
    {
        public Bud (Mesh mesh, LifeCyleStage next) : base(mesh, next) { Stage = Stage.Bud; }
    }

    public class Flower : LifeCyleStage
    {
        public Flower (Mesh mesh, LifeCyleStage next) : base(mesh, next) { Stage = Stage.Flower; }
    }

    public class Fruit : LifeCyleStage
    {
        public Fruit (Mesh mesh, LifeCyleStage next) : base(mesh, next) { Stage = Stage.Fruit; }
    }
}