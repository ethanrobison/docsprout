using Code.Characters.Doods.Needs;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.Doodex
{
    public class Doodex
    {
        private GameObject _go;
        private bool _active;
        private DoodStatus _activeStatus;

        public void Show (DoodStatus status) {
            Logging.Assert(!_active, "Double showing doodex?!");

            _go = UIUtils.MakeUIPrefab(UIPrefab.Doodex);
            _activeStatus = status;
            OnShow();

            _active = true;
        }

        public void Hide () {
            OnHide();
            Object.Destroy(_go);
            _go = null;

            _active = false;
        }

        private void OnShow () { }

        private void OnHide () { }
    }
}