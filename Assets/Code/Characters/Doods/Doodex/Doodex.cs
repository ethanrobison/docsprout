using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Characters.Doods.Doodex
{
    public class Doodex
    {
        private GameObject _go;
        private bool _active;
        private Dood _activeDood;

        private DoodexTab _doodTab, _marketTab;

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
//            _market.OnInitialize();
            _doodTab = new DoodTab(_go.transform.Find("Tabs/Dood").gameObject, _activeDood);
            _doodTab.OnInitialize();
        }

        private void OnHide () {
            _doodTab.OnShutdown();
//            _market.OnShutdown();
        }
    }

    public abstract class DoodexTab
    {
        protected readonly GameObject GO;
        protected readonly Dood Dood;

        protected DoodexTab (GameObject go, Dood dood) {
            GO = go;
            Dood = dood;
        }

        public void SetState (bool state) { GO.SetActive(state); }

        public abstract void OnInitialize ();
        public abstract void OnShutdown ();

        public virtual void Show () { }
        public virtual void Hide () { }
    }
}