using System.Collections.Generic;
using Code.Characters.Doods.Needs;
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

        public static MeshInfo GetLeafOfType (Maturity stage) {
            ModelInfo info;
            if (!LeafPaths.TryGetValue(stage, out info)) { Logging.Error("Missing path for type: " + stage); }

            return new MeshInfo {Mesh = LoadMeshAtPath(info.Path), Offset = info.Pos};
        }

        private static Mesh LoadMeshAtPath (string path) {
            var go = (GameObject) Resources.Load(path);
            Logging.Assert(go != null, "Missing model at path: " + path);

            if (go == null) { return null; }

            var mesh = go.GetComponent<MeshFilter>().sharedMesh;
            Logging.Assert(mesh != null, "Missing mesh at path: " + path);

            return mesh;
        }

        //
        // Nasty hard-coded dictionaries. Sorry about this.

        private static readonly Dictionary<BodyType, string> BodyPaths = new Dictionary<BodyType, string> {
            {BodyType.Cone, "Models/Doods/cone2"},
            {BodyType.Capsule, "Models/Doods/capsule2"}
        };


        private const string PLANT_BASE = "Models/Doods/Plants/";

        private static readonly Vector3
            CapsuleOffset = new Vector3(0, 0, 1.8f),
            ConeOffset = new Vector3(0, 0, 0.9f);

        private static readonly Dictionary<Maturity, ModelInfo> LeafPaths = new Dictionary<Maturity, ModelInfo> {
            {Maturity.Empty, new ModelInfo {Path = PLANT_BASE + "sprout1", Pos = ConeOffset}},
            {Maturity.Seedling, new ModelInfo {Path = PLANT_BASE + "sprout1", Pos = CapsuleOffset}},
            {Maturity.Sprout, new ModelInfo {Path = PLANT_BASE + "sprout2", Pos = CapsuleOffset}}
        };
        
        private static Dictionary<DoodSpecies, DoodSpeciesInfo> _doodSpeciesInfo;

        private struct ModelInfo
        {
            public string Path;
            public Vector3 Pos;

            public ModelInfo (string path, Vector3 pos) {
                Path = path;
                Pos = pos;
            }
        }
    }

    public struct MeshInfo
    {
        public Mesh Mesh;
        public Vector3 Offset;
    }
}