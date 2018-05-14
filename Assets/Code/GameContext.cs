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

        public void StartGame (SceneIndex index) {
            Initialize();
            if (index < SceneIndex.Demo) { return; }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene((int) index);
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
                Doods.DoodList[0].gameObject.GetRequiredComponent<Growth>().Species = Species.NoNeeds;
            }
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