using System.Collections.Generic;
using System.IO;
using Code.Characters.Doods.LifeCycle;
using UnityEditor;
using UnityEngine;
using Code.Characters.Doods.Needs;

namespace Editor
{
    public class DoodEditor : EditorWindow
    {
        private const string SAVE_FILE = "Resources/Doods/DoodSpecies/Species.json";
        private Doodopedia.DoodSpecies _species;
        [SerializeField] private List<DoodSpeciesSerializable> _doodSpecies;
        private string _filePath;


        private Dictionary<Doodopedia.DoodSpecies, SpeciesEditor> _speciesEditors;

        [MenuItem("Window/Dood Editor")]
        private static void Init () {
            // Get existing open window or if none, make a new one:
            DoodEditor window = (DoodEditor) EditorWindow.GetWindow(typeof(DoodEditor));
            window.Show();
        }


        private void Awake () {
            _speciesEditors = new Dictionary<Doodopedia.DoodSpecies, SpeciesEditor>();
            _filePath = Path.Combine(Application.dataPath, SAVE_FILE);
            if (!File.Exists(_filePath)) {
                File.Create(_filePath).Close();
                _doodSpecies = new List<DoodSpeciesSerializable>();
                Save();
                return;
            }

            StreamReader sr = new StreamReader(_filePath);
            _doodSpecies = JsonUtility.FromJson<SpeciesContainer>(sr.ReadLine()).Species;
            sr.Close();
            sr.Dispose();

            foreach (var species in _doodSpecies) {
                _speciesEditors.Add(_species, new SpeciesEditor(species));
            }
        }


        private void OnGUI () {
            if (GUILayout.Button("Save")) {
                Save();
            }

            _species = (Doodopedia.DoodSpecies) EditorGUILayout.EnumPopup("Species", _species);
            if (!_speciesEditors.ContainsKey(_species)) {
                var newSpecies = new DoodSpeciesSerializable {Species = _species};
                _speciesEditors.Add(_species, new SpeciesEditor(newSpecies));
                _doodSpecies.Add(newSpecies);
            }

            _speciesEditors[_species].Show();
        }

        private void Save () {
            var sw = new StreamWriter(_filePath, false);
            sw.WriteLine(JsonUtility.ToJson(new SpeciesContainer(_doodSpecies)));
            sw.Close();
            sw.Dispose();
        }
    }


    public class SpeciesEditor
    {
        private DoodSpeciesSerializable _doodSpecies;
        private Maturity _maturity;
        private Dictionary<Maturity, LifeCylceEditor> _cylceEditors = new Dictionary<Maturity, LifeCylceEditor>();

        public SpeciesEditor (DoodSpeciesSerializable doodSpecies) {
            _doodSpecies = doodSpecies;
            if (_doodSpecies.Cycles == null) {
                _doodSpecies.Cycles = new List<LifeCycleInfoSerializable>();
                return;
            }

            foreach (var cycle in _doodSpecies.Cycles) {
                _cylceEditors.Add(cycle.Current, new LifeCylceEditor(cycle));
            }
        }

        public void Show () {
            EditorGUILayout.BeginVertical();
            _doodSpecies.BodyType = (BodyType) EditorGUILayout.EnumPopup("Body", _doodSpecies.BodyType);
            _doodSpecies.StageAfterHarvest = (Maturity) EditorGUILayout.EnumPopup("Stage After Harvest",
                _doodSpecies.StageAfterHarvest);

            EditorGUI.indentLevel++;
            _maturity = (Maturity) EditorGUILayout.EnumPopup("Life Cycle Stage", _maturity);
            if (!_cylceEditors.ContainsKey(_maturity)) {
                var newCycle = new LifeCycleInfoSerializable {Current = _maturity};
                _cylceEditors.Add(_maturity, new LifeCylceEditor(newCycle));
                _doodSpecies.Cycles.Add(newCycle);
            }

            _cylceEditors[_maturity].Show();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

    public class LifeCylceEditor
    {
        private LifeCycleInfoSerializable _lifeCycle;
        private NeedType _need = NeedType.Water;

        private Dictionary<NeedType, NeedValuesEditor> _needValuesEditors =
            new Dictionary<NeedType, NeedValuesEditor>();

        public LifeCylceEditor (LifeCycleInfoSerializable lifeCycle) {
            _lifeCycle = lifeCycle;
            if (_lifeCycle.NeedValues == null) {
                _lifeCycle.NeedValues = new List<NeedValuesInfoSerializable>();
                return;
            }

            foreach (var needValue in _lifeCycle.NeedValues) {
                _needValuesEditors.Add(needValue.Type, new NeedValuesEditor(needValue));
            }
        }

        public void Show () {
            EditorGUILayout.BeginVertical();
            _lifeCycle.Next = (Maturity) EditorGUILayout.EnumPopup("Next Stage", _lifeCycle.Next);
            _lifeCycle.GrowthRate = EditorGUILayout.FloatField("Growth Rate", _lifeCycle.GrowthRate);
            EditorGUI.indentLevel++;
            _need = (NeedType) EditorGUILayout.EnumPopup("Need", _need);
            if (!_needValuesEditors.ContainsKey(_need)) {
                var newNeed = new NeedValuesInfoSerializable {Type = _need};
                _needValuesEditors.Add(_need, new NeedValuesEditor(newNeed));
                _lifeCycle.NeedValues.Add(newNeed);
            }

            _needValuesEditors[_need].Show();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }

    public class NeedValuesEditor
    {
        private NeedValuesInfoSerializable _needValues;
        public NeedValuesEditor (NeedValuesInfoSerializable needValues) { _needValues = needValues; }

        public void Show () {
            EditorGUILayout.BeginVertical();
            _needValues.Top = EditorGUILayout.FloatField("Optimal Range Top", _needValues.Top);
            _needValues.Bottom = EditorGUILayout.FloatField("Optimal Range Bottom", _needValues.Bottom);
            _needValues.SatisfactionRate = EditorGUILayout.FloatField("Satisfaction Rate",
                _needValues.SatisfactionRate);
            _needValues.DecayRate = EditorGUILayout.FloatField("Decay Rate",
                _needValues.DecayRate);
            EditorGUILayout.EndVertical();
        }
    }

    [System.Serializable]
    public class SpeciesContainer
    {
        public List<DoodSpeciesSerializable> Species;
        public SpeciesContainer (List<DoodSpeciesSerializable> species) { Species = species; }
    }

    [System.Serializable]
    public class DoodSpeciesSerializable
    {
        public Doodopedia.DoodSpecies Species;
        public Maturity StageAfterHarvest;
        public BodyType BodyType;
        public List<LifeCycleInfoSerializable> Cycles;
    }

    [System.Serializable]
    public class LifeCycleInfoSerializable
    {
        public Maturity Current;
        public bool Enabled;
        public Maturity Next;
        public float GrowthRate;
        public List<NeedValuesInfoSerializable> NeedValues;
    }

    [System.Serializable]
    public class NeedValuesInfoSerializable
    {
        public NeedType Type;
        public bool Enabled;
        public float Bottom, Top, SatisfactionRate, DecayRate;
    }
}