using Code.Characters.Doods;
using Code.Characters.Player;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameContext
    {
        public DoodManager Doods { get; private set; }
        public Player Player { get; private set; }

        public void StartGame (int scene, bool testscene = false) {
            if (testscene) {
                Initialize();
                return;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(scene);

            Initialize();
        }

        public void SetPlayer (Player player) { Player = player; }

        private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
            Initialize();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Initialize () {
            Doods = new DoodManager();

            Doods.Initialize();
        }

        private void ShutDown () {
            Doods.ShutDown();

            Doods = null;
        }

        // todo exit a game and return to the main menu
    }

    public interface IContextManager
    {
        void Initialize ();
        void ShutDown ();
    }
}