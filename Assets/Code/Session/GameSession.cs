using UnityEngine.SceneManagement;

namespace Code.Session
{
    public class GameSession
    {
        public PreferencesManager Prefs { get; private set; }
        public InputManager Input { get; private set; }

        public void Initialize () {
            Prefs = new PreferencesManager();
            Input = new InputManager();

            Prefs.Initialize();
            Input.Initialize();
        }


        // todo this isn't very sophisticated in fact I dislike it
        public void StartGame (int scene) {
            var ctx = new GameContext();
            Game.SetContext(this, ctx);

            ctx.StartGame(scene);

            Input.OnGameStart();
            Prefs.OnGameStart();
        }

        public void StartTestGame () {
            var ctx = new GameContext();
            Game.SetContext(this, ctx);

            ctx.StartGame(0, true);

            Input.OnGameStart();
            Prefs.OnGameStart();
        }

        private void StopGame () { Input.OnGameStop(); }

        public void ReturnToMenu () {
            StopGame();
            SceneManager.sceneLoaded += RemoveContext;
            SceneManager.LoadScene(0);
        }

        private static void RemoveContext (Scene scene, LoadSceneMode mode) {
            Game.Ctx = null;
            SceneManager.sceneLoaded -= RemoveContext;
        }
    }

    public interface ISessionManager
    {
        void Initialize ();
        void ShutDown ();
        void OnGameStart ();
        void OnGameStop ();
    }
}