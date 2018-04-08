using Code.Utils;
using UnityEngine;

namespace Code.Session.UI
{
    /// <summary>
    /// Stub class, specifically for menus outside of the game context.
    /// Not to be used when a game is actually running.
    /// </summary>
    public abstract class Menu : ISmartStackElement
    {
        protected GameObject GO;

        protected abstract void CreateGameObject ();

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