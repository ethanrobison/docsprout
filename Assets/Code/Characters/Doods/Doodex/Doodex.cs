using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Doodex
{
    public class Doodex
    {
        private GameObject _go;
        private bool _active;
        private Dood _activeDood;

        private Transform _needs;

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
        }

        private void OnHide () { }
    }

    public class NeedMonitor : MonoBehaviour
    {
        public Need Need;

        private void Update () { }
    }
}