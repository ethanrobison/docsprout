using Code.Characters.Doods.LifeCycle;
using UnityEngine.SceneManagement;

namespace Code.Session
{
    public enum SceneIndex
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

            SetCtx(SceneIndex.Current);
        }


        // todo this isn't very sophisticated in fact I dislike it
        public void StartGame (SceneIndex index) {
            SetCtx(index);
            Game.Ctx.StartGame((int) index);
            Input.OnGameStart();
            Prefs.OnGameStart();
        }

        private void StopGame () {
            Input.OnGameStop();
            Prefs.OnGameStop();
        }

        public void ReturnToMenu () {
            SetCtx(SceneIndex.MainMenu);
            StopGame();
        }

        private void SetCtx (SceneIndex index) {
            Game.SetContext(this, index == SceneIndex.MainMenu ? null : new GameContext());
        }


//        public void ReturnToMenu () {
//            StopGame();
//            SceneManager.sceneLoaded += RemoveContext;
//            SceneManager.LoadScene(0);
//        }
//
//        private static void RemoveContext (Scene scene, LoadSceneMode mode) {
//            Game.Ctx = null;
//            SceneManager.sceneLoaded -= RemoveContext;
//        }
    }

    public interface ISessionManager
    {
        void Initialize ();
        void ShutDown ();
        void OnGameStart ();
        void OnGameStop ();
    }
}