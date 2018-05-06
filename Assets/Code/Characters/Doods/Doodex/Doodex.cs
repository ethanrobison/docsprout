using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Characters.Doods.Doodex
{
    public class Doodex
    {
        private bool _active;
        private GameObject _go;
        private Dood _activeDood;
        private Transform _needs;

        private GameObject _doodovision;

        public void Show (Dood dood) {
            if (_active) { return; }

            _go = UIUtils.MakeUIPrefab(UIPrefab.Doodex);
            _activeDood = dood;
            OnShow();

            _active = true;
        }

        public void Hide () {
            if (!_active) { return; }

            OnHide();
            Object.Destroy(_go);
            _go = null;

            _active = false;
        }

        private void OnShow () {
            _needs = _go.transform.Find("Left/Needs");
            foreach (var need in _activeDood.Comps.Status.Needs) {
                var meter = UIUtils.MakeUIPrefab(UIPrefab.NeedMeter, _needs);
                var monitor = meter.AddComponent<NeedMonitor>();
                monitor.Need = need;
            }

            var prefab = Resources.Load("Doods/Doodovision");
            _doodovision = (GameObject) Object.Instantiate(prefab, _activeDood.transform);
        }

        private void OnHide () {
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