using System.Collections.Generic;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.LifeCycle
{
    public enum BodyType
    {
        Cone,
        Capsule,
        Sphere,
        Cube,
        Cylinder
    }

    public static class Doodopedia
    {
        public enum DoodSpecies
        {
            Debug,
        }

        public static LifeCycleStage MakeDoodOfType (DoodSpecies species) {
            var capsule = GetBodyOfType(BodyType.Capsule);
            var cone = GetBodyOfType(BodyType.Cone);

            var bud = new LifeCycleStage(BodyType.Capsule, Maturity.Bud, null);
            var sprout = new LifeCycleStage(BodyType.Capsule, Maturity.Sprout, bud);
            var seedling = new LifeCycleStage(BodyType.Cone, Maturity.Seedling, sprout);
            var seed = new LifeCycleStage(BodyType.Cone, Maturity.Seed, seedling);
            return seed;
        }


        public static Mesh GetBodyOfType (BodyType type) {
            string path;
            if (!BodyPaths.TryGetValue(type, out path)) { Logging.Error("Missing path for type: " + type); }

            return LoadMeshAtPath(path);
        }

        public static Mesh GetLeafOfType (Maturity stage) {
            string path;
            if (!LeafPaths.TryGetValue(stage, out path)) { Logging.Error("Missing path for type: " + stage); }

            return LoadMeshAtPath(path);
        }

        private static Mesh LoadMeshAtPath (string path) {
            var go = (GameObject) Resources.Load(path);
            Logging.Assert(go != null, "Missing model at path: " + path);

            if (go == null) { return null; }

            var mesh = go.GetComponent<MeshFilter>().sharedMesh;
            Logging.Assert(mesh != null, "Missing mesh at path: " + path);

            return mesh;
        }

        private static readonly Dictionary<BodyType, string> BodyPaths = new Dictionary<BodyType, string> {
            { BodyType.Cone, "Models/Doods/cone2" },
            { BodyType.Capsule, "Models/Doods/capsule2" }
        };

        private static readonly Dictionary<Maturity, string> LeafPaths = new Dictionary<Maturity, string> {
            { Maturity.Empty, "Models/Doods/Plants/sprout1" },
            { Maturity.Seedling, "Models/Doods/Plants/sprout1" },
            { Maturity.Sprout, "Models/Doods/Plants/sprout2" }
        };
    }
}