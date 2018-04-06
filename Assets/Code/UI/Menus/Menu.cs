using Code.Utils;
using UnityEngine;

namespace Code.UI.Menus
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


        public virtual void OnPush () { }

        public virtual void Activate () {
            CreateGameObject();
        }

        public virtual void Deactivate () {
            RemoveGameObject();
        }

        public virtual void OnPop () { }
    }
}