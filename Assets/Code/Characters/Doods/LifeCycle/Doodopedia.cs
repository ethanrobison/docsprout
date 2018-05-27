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
        Bush = 4,
        Tree = 5,
        Cactus1 = 6,
        Cactus2 = 7,
        Cactus3 = 8
    }

    public enum Species
    {
        MainMenu = -1,
        Vine,
        Bush,
        Tree,
        Cactus
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
                Material = LoadMaterialAtPath(leafInfo.Material),
                Scale = leafInfo.Scale
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

        public static readonly Dictionary<Species, string> FrootPaths = new Dictionary<Species, string> {
            { Species.Vine, "Doods/Froot/SproutFroot" },
            { Species.Bush, "Doods/Froot/BushFroot" },
            { Species.Tree, "Doods/Froot/TreeFroot" },
            { Species.Cactus, "Doods/Froot/CactusFroot" }
        };

        //
        // Nasty hard-coded dictionaries. Sorry about this.

        private static readonly Dictionary<BodyType, string> BodyPaths = new Dictionary<BodyType, string> {
            { BodyType.Capsule, "Models/Doods/CapsuleScaled" },
            { BodyType.Cone, "Models/Doods/cone2" },
            { BodyType.Cube, "Models/Doods/cube2" },
            { BodyType.Cylinder, "Models/Doods/cylinder2" },
            { BodyType.Sphere, "Models/Doods/sphere2" }
        };


        private const string PLANT_BASE = "Models/Doods/Plants/";
        private const string MATERIAL_BASE = "Graphics/Materials/";

        private static readonly Dictionary<BodyType, Vector3> LeafOffsets = new Dictionary<BodyType, Vector3> {
            { BodyType.Capsule, new Vector3(0, 0, 1.8f) },
            { BodyType.Cone, new Vector3(0, 0, 0.9f) },
            { BodyType.Cube, new Vector3(0, 0, 1f) },
            { BodyType.Cylinder, new Vector3(0, 0, 0.9f) },
            { BodyType.Sphere, new Vector3(0, 0, 0.8f) }
        };

        private static readonly Dictionary<LeafType, LeafInfo> LeafPaths = new Dictionary<LeafType, LeafInfo> {
            { LeafType.Seed, new LeafInfo(PLANT_BASE + "Seed", MATERIAL_BASE + "Seed") },
            { LeafType.Seedling, new LeafInfo(PLANT_BASE + "sprout1", MATERIAL_BASE + "Sprout") },
            { LeafType.Sprout, new LeafInfo(PLANT_BASE + "sprout2", MATERIAL_BASE + "Sprout") },
            { LeafType.Bush, new LeafInfo(PLANT_BASE + "bush", MATERIAL_BASE + "peachTree", 1.4f) },
            { LeafType.Tree, new LeafInfo(PLANT_BASE + "tree", MATERIAL_BASE + "peachTree", 0.8f) },
            { LeafType.Cactus1, new LeafInfo(PLANT_BASE + "Cactus1", MATERIAL_BASE + "Sprout", 0.8f) },
            { LeafType.Cactus2, new LeafInfo(PLANT_BASE + "Cactus2", MATERIAL_BASE + "Sprout", 0.8f) },
            { LeafType.Cactus3, new LeafInfo(PLANT_BASE + "Cactus3", MATERIAL_BASE + "Sprout", 0.8f) }
        };

        private static readonly Dictionary<Species, DoodSpecies> SpeciesInstances =
            new Dictionary<Species, DoodSpecies>();

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

            SpeciesInstances.Add(Species.Vine,
                new DoodSpecies(Species.Vine, BodyType.Capsule, Maturity.Sprout, cycles));

            var noNeedCycles = new SpeciesLifeCycles { LifeCycles = new List<MaturityLifeCyclePair>() };
            var noNeedNeeds = new LifeCycleNeeds { Needs = new List<NeedType>() };

            noNeedCycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seed,
                new LifeCycleStage(new LifeCycleValues(Maturity.Empty, 0, LeafType.Seed), noNeedNeeds)));

            SpeciesInstances.Add(Species.MainMenu,
                new DoodSpecies(Species.MainMenu, BodyType.Capsule, Maturity.Seed, noNeedCycles));

            //// bush

            cycles = new SpeciesLifeCycles { LifeCycles = new List<MaturityLifeCyclePair>() };

            needs = new LifeCycleNeeds {
                Needs = new List<NeedType> { NeedType.Water }
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seed,
                new LifeCycleStage(new LifeCycleValues(Maturity.Seedling, 1, LeafType.Seed), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seedling,
                new LifeCycleStage(new LifeCycleValues(Maturity.Sprout, 4, LeafType.Seedling), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun,
                NeedType.Food
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Sprout,
                new LifeCycleStage(new LifeCycleValues(Maturity.Fullgrown, 6, LeafType.Bush), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun,
                NeedType.Food
            };

            newCycle =
                new LifeCycleStage(new LifeCycleValues(Maturity.Empty, 0, LeafType.Sprout), needs) {
                    Values = { Harvestable = true }
                };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Fullgrown, newCycle));

            SpeciesInstances.Add(Species.Bush,
                new DoodSpecies(Species.Bush, BodyType.Cone, Maturity.Sprout, cycles));

            //// tree

            cycles = new SpeciesLifeCycles { LifeCycles = new List<MaturityLifeCyclePair>() };

            needs = new LifeCycleNeeds {
                Needs = new List<NeedType> { NeedType.Water }
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seed,
                new LifeCycleStage(new LifeCycleValues(Maturity.Seedling, 1, LeafType.Seed), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seedling,
                new LifeCycleStage(new LifeCycleValues(Maturity.Sprout, 5, LeafType.Seedling), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun,
                NeedType.Food
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Sprout,
                new LifeCycleStage(new LifeCycleValues(Maturity.Fullgrown, 7, LeafType.Tree), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Water,
                NeedType.Sun,
                NeedType.Food
            };

            newCycle =
                new LifeCycleStage(new LifeCycleValues(Maturity.Empty, 0, LeafType.Sprout), needs) {
                    Values = { Harvestable = true }
                };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Fullgrown, newCycle));

            SpeciesInstances.Add(Species.Tree,
                new DoodSpecies(Species.Tree, BodyType.Cube, Maturity.Sprout, cycles));

            //// cactus

            cycles = new SpeciesLifeCycles { LifeCycles = new List<MaturityLifeCyclePair>() };

            needs = new LifeCycleNeeds {
                Needs = new List<NeedType> { NeedType.Sun }
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seed,
                new LifeCycleStage(new LifeCycleValues(Maturity.Seedling, 1, LeafType.Cactus1), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Sun
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Seedling,
                new LifeCycleStage(new LifeCycleValues(Maturity.Sprout, 3, LeafType.Cactus2), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Sun,
                NeedType.Food
            };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Sprout,
                new LifeCycleStage(new LifeCycleValues(Maturity.Fullgrown, 5, LeafType.Cactus3), needs)));

            needs.Needs = new List<NeedType> {
                NeedType.Sun,
                NeedType.Food
            };

            newCycle =
                new LifeCycleStage(new LifeCycleValues(Maturity.Empty, 0, LeafType.Sprout), needs) {
                    Values = { Harvestable = true }
                };

            cycles.LifeCycles.Add(new MaturityLifeCyclePair(Maturity.Fullgrown, newCycle));

            SpeciesInstances.Add(Species.Cactus,
                new DoodSpecies(Species.Cactus, BodyType.Cylinder, Maturity.Sprout, cycles));
        }

        private struct LeafInfo
        {
            public string Model;
            public string Material;
            public float Scale;

            public LeafInfo (string model, string material, float scale = 1f) {
                Model = model;
                Material = material;
                Scale = scale;
            }
        }
    }

    public struct MeshInfo
    {
        public Mesh Mesh;
        public Material Material;
        public Vector3 Offset;
        public float Scale;
    }
}