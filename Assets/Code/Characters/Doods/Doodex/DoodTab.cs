using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Characters.Doods.Doodex
{
    public class DoodTab : DoodexTab
    {
        private readonly Transform _needs;
        private GameObject _doodovision;

        public DoodTab (GameObject go, Dood dood) : base(go, dood) {
            _needs = GO.transform.Find("Left/Needs");

            foreach (var need in Dood.Comps.Status.Needs) {
                var meter = UIUtils.MakeUIPrefab(UIPrefab.NeedMeter, _needs);
                var monitor = meter.AddComponent<NeedMonitor>();
                monitor.Need = need;
            }

            var prefab = Resources.Load("Doods/Doodovision");
            _doodovision = (GameObject) Object.Instantiate(prefab, Dood.transform);
        }

        public override void OnInitialize () { }

        public override void OnShutdown () {
            Object.Destroy(_doodovision);
            _doodovision = null;
        }
    }


    public class NeedMonitor : MonoBehaviour
    {
        public Need Need;

        private Transform _meter;

        private void Start () {
            _meter = transform.Find("Meter");
            transform.Find("Label").GetComponent<Text>().text = Need.Type.ToString();
        }

        private void Update () {
            var value = Mathf.RoundToInt(Need.Value / 25f);
            SetToggleOn(value);
        }

        private void SetToggleOn (int index) { _meter.GetChild(index).GetComponent<Toggle>().isOn = true; }
    }
}