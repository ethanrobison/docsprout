using Code.Characters.Doods.LifeCycle;
using UnityEngine.SceneManagement;

namespace Code.Session
{
    public enum SceneIdx
    {
        Current = -1,
        MainMenu = 0,
        Demo = 1,
        Ethan = 2,
        Kyle = 3,
        Alyssa = 4
    }

    public class GameSession
    {
        public PreferencesManager Prefs { get; private set; }
        public InputManager Input { get; private set; }

        public void Initialize () {
            Prefs = new PreferencesManager();
            Input = new InputManager();

            Prefs.Initialize();
            Input.Initialize();
            Doodopedia.LoadSpecies();

            SetCtx((int) SceneIdx.Current);
        }

        //        private void Shutdown () {
        //            Dialogs.ShutDown();
        //            Prefs.ShutDown();
        //
        //            Dialogs = null;
        //            Prefs = null;
        //        }


        // todo this isn't very sophisticated in fact I dislike it
        public void StartGame (int scene) { SetCtx(scene); }

        public void ReturnToMenu () { SetCtx((int) SceneIdx.MainMenu); }

        private void SetCtx (int scene) {
            var ctx = new GameContext();
            Game.SetContext(this, ctx);

            ctx.StartGame(scene);

            Input.OnGameStart();
            Prefs.OnGameStart();
        }
    }

    public interface ISessionManager
    {
        void Initialize ();
        void ShutDown ();
        void OnGameStart ();
    }
}