using Code.Utils;
using UnityEngine;

namespace Code.UI
{
    public abstract class BaseDialog : ISmartStackElement
    {
        protected GameObject GO;

        private readonly UIPrefab _prefab;


        protected BaseDialog (UIPrefab prefab) {
            _prefab = prefab;
        }

        protected virtual void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(_prefab);
        }

        protected virtual void RemoveGameObject () {
            Object.Destroy(GO);
            GO = null;
        }


        public virtual void OnPush () {
            CreateGameObject();
        }

        public virtual void Activate () {
            GO.SetActive(true);
        }

        public virtual void Deactivate () {
            GO.SetActive(false);
        }

        public virtual void OnPop () {
            RemoveGameObject();
        }
    }
}