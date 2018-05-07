using System.Collections.Generic;
using System.Linq;
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
        public static Mesh GetBodyOfType (BodyType type) {
            string path;
            if (!BodyPaths.TryGetValue(type, out path)) { Logging.Error("Missing path for type: " + type); }

            return LoadMeshAtPath(path);
        }

        public static MeshInfo GetLeafForBody (LeafType leaf, BodyType bodyType) {
            LeafInfo leafInfo;
            if (!LeafPaths.TryGetValue(leaf, out leafInfo)) { Logging.Error("Missing path for type: " + leaf); }

            return new MeshInfo {
                Mesh = LoadMeshAtPath(leafInfo.Model),
                Offset = LeafOffsets[bodyType],
                Material = LoadMaterialAtPath(leafInfo.Material)
            };
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

        private static Material LoadMaterialAtPath (string path) {
            var mat = (Material) Resources.Load(path);
            Logging.Assert(mat != null, "Missing material at path " + path);
            return mat;
        }

        //
        // Nasty hard-coded dictionaries. Sorry about this.

        private static readonly Dictionary<BodyType, string> BodyPaths = new Dictionary<BodyType, string> {
            { BodyType.Cone, "Models/Doods/cone2" },
            { BodyType.Capsule, "Models/Doods/CapsuleScaled" }
        };


        private const string PLANT_BASE = "Models/Doods/Plants/";
        private const string MATERIAL_BASE = "Graphics/Materials/";

        private static readonly Dictionary<BodyType, Vector3> LeafOffsets = new Dictionary<BodyType, Vector3> {
            { BodyType.Capsule, new Vector3(0, 0, 1.8f) },
            { BodyType.Cone, new Vector3(0, 0, 0.9f) }
        };

        private static readonly Dictionary<LeafType, LeafInfo> LeafPaths = new Dictionary<LeafType, LeafInfo> {
            { LeafType.Seed, new LeafInfo { Model = PLANT_BASE + "Seed", Material = MATERIAL_BASE + "Seed" } },
            { LeafType.Seedling, new LeafInfo { Model = PLANT_BASE + "sprout1", Material = MATERIAL_BASE + "Sprout" } },
            { LeafType.Sprout, new LeafInfo { Model = PLANT_BASE + "sprout2", Material = MATERIAL_BASE + "Sprout" } }
        };

        private static Dictionary<Species, DoodSpecies> SpeciesInstances = new Dictionary<Species, DoodSpecies>();

        public static void LoadSpecies () {
            var cycles = new SpeciesLifeCycles { LifeCycles = new List<MaturityLifeCyclePair>() };

            var needs = new LifeCycleNeeds {
                Needs = new List<NeedType> { NeedType.Water }
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seed,
                new LifeCycleStage(new LifeCycleValues(Maturity.Seedling, 1, LeafType.Seed), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seedling,
                new LifeCycleStage(new LifeCycleValues(Maturity.Sprout, 3, LeafType.Seedling), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun,
                NeedType.Food
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Sprout,
                new LifeCycleStage(new LifeCycleValues(Maturity.Fullgrown, 5, LeafType.Sprout), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun,
                NeedType.Food
            };

            var newCycle =
                new LifeCycleStage(new LifeCycleValues(Maturity.Empty, 0, LeafType.Sprout), needs) {
                    Values = { Harvestable = true }
                };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Fullgrown, newCycle));

            SpeciesInstances.Add(Species.Debug,
                new DoodSpecies(Species.Debug, BodyType.Capsule, Maturity.Sprout, cycles));
        }

        private struct LeafInfo
        {
            public string Model;
            public string Material;
        }
    }

    public struct MeshInfo
    {
        public Mesh Mesh;
        public Material Material;
        public Vector3 Offset;
    }
}