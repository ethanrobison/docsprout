using UnityEngine;

namespace Code.UI.Menus
{
    /// <summary>
    /// Stub class, specifically for menus outside of the game context.
    /// Not to be used when a game is actually running.
    /// </summary>
    public abstract class Menu
    {
        protected GameObject GO;

        public abstract void CreateGameObject ();
        public abstract void RemoveGameObject ();
    }
}