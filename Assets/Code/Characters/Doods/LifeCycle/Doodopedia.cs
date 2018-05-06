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

    public enum LeafType
    {
        Seed = 0,
        Seedling = 1,
        Sprout = 2,
        Bud = 3,
    }

    public enum Species
    {
        Debug,
    }

    public static class Doodopedia
    {
//        public static LifeCycleStage MakeDoodOfType (Species species) {
//            var capsule = GetBodyOfType(BodyType.Capsule);
//            var cone = GetBodyOfType(BodyType.Cone);
//
//            var bud = new LifeCycleStage(BodyType.Capsule, Maturity.Bud, null);
//            var sprout = new LifeCycleStage(BodyType.Capsule, Maturity.Sprout, bud);
//            var seedling = new LifeCycleStage(BodyType.Cone, Maturity.Seedling, sprout);
//            var seed = new LifeCycleStage(BodyType.Cone, Maturity.Seed, seedling);
//            return seed;
//        }


        public static Mesh GetBodyOfType (BodyType type) {
            string path;
            if (!BodyPaths.TryGetValue(type, out path)) { Logging.Error("Missing path for type: " + type); }

            return LoadMeshAtPath(path);
        }

        public static MeshInfo GetLeafForBody (LeafType leaf, BodyType bodyType) {
            string path;
            if (!LeafPaths.TryGetValue(leaf, out path)) { Logging.Error("Missing path for type: " + leaf); }

            return new MeshInfo {Mesh = LoadMeshAtPath(path), Offset = LeafOffsets[bodyType]};
        }

        public static DoodSpecies GetDoodSpecies (Species species) { return SpeciesInstances[species]; }

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

        private static readonly Dictionary<BodyType, Vector3> LeafOffsets = new Dictionary<BodyType, Vector3> {
            {BodyType.Capsule, new Vector3(0, 0, 1.8f)},
            {BodyType.Cone, new Vector3(0, 0, 0.9f)}
        };

        private static readonly Dictionary<LeafType, string> LeafPaths = new Dictionary<LeafType, string> {
            {LeafType.Seed, PLANT_BASE + "Seed"},
            {LeafType.Seedling, PLANT_BASE + "sprout1"},
            {LeafType.Sprout, PLANT_BASE + "sprout2"}
        };

        private static Dictionary<Species, DoodSpecies> SpeciesInstances = new Dictionary<Species, DoodSpecies>();

        public static void LoadSpecies () {
            var cycles = new SpeciesLifeCycles();
            cycles.LifeCycles = new List<MaturityLifeCyclePair>();
            var needs = new LifeCycleNeeds();
            needs.Needs = new List<NeedTypeIntPair> {
                new NeedTypeIntPair(NeedType.Water, 1),
                new NeedTypeIntPair(NeedType.Sun, 0),
                new NeedTypeIntPair(NeedType.Fun, 0)
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seed,
                new LifeCycleStage(new LifeCycleValues(Maturity.Seedling, 1, LeafType.Seed), needs)));

            needs.Needs = new List<NeedTypeIntPair> {
                new NeedTypeIntPair(NeedType.Water, 1),
                new NeedTypeIntPair(NeedType.Sun, 1),
                new NeedTypeIntPair(NeedType.Fun, 0)
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seedling,
                new LifeCycleStage(new LifeCycleValues(Maturity.Empty, 0, LeafType.Seedling), needs)));

            SpeciesInstances.Add(Species.Debug, new DoodSpecies(Species.Debug, BodyType.Capsule, cycles));
        }
    }

    public struct MeshInfo
    {
        public Mesh Mesh;
        public Vector3 Offset;
    }
}