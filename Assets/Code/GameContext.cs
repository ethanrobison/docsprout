using Code.Characters.Doods;
using Code.Characters.Doods.LifeCycle;
using Code.Characters.Doods.Needs;
using Code.Characters.Player;
using Code.Session;
using Code.Utils;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameContext
    {
        public DoodManager Doods { get; private set; }
        public Player Player { get; private set; }
        public EconomyManager Economy { get; private set; }
        public HUDManager HUD { get; private set; }

        public bool InMenu {
            get { return _index == SceneIndex.MainMenu || _index == SceneIndex.Current; }
        }

        private SceneIndex _index;
        private bool _initialized;

        public void StartGame (SceneIndex index) {
            _index = index;
            if (_index == SceneIndex.Current) {
                Initialize();
                return;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene((int) _index);
        }

        public void SetPlayer (Player player) {
            Player = player;
            Doods.DoodList[0].transform.position = player.transform.position + player.transform.forward * 3f;
        }

        private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
            Initialize();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Initialize () {
            Doods = new DoodManager();
            Doods.Initialize();

            //All of this feels like a hack, but I don't have any better ideas
            if (SceneManager.GetActiveScene().buildIndex != (int) SceneIndex.MainMenu) {
                Doods.DoodList[0].IsSelected = true;
                Doods.DoodList[0].transform.Find("Dood/Body").gameObject.GetRequiredComponent<DoodColor>().IsSelected =
                    true;
            }
            else {
                // The menu must not be sad
                Doods.DoodList[0].gameObject.GetRequiredComponent<Growth>().Species = Species.MainMenu;
            }

            Economy = new EconomyManager();
            Economy.Initialize();

            HUD = new HUDManager();
            HUD.Initialize();

            _initialized = true;
        }

        public void ShutDown () {
            if (!_initialized) { return; }

            HUD.ShutDown();
            HUD = null;

            Economy.ShutDown();
            Economy = null;

            Doods.ShutDown();
            Doods = null;
            _initialized = false;
        }
    }

    public interface IContextManager
    {
        void Initialize ();
        void ShutDown ();
    }
}