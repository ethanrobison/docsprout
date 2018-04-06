using Code.Doods;
using Code.UI;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameContext
    {
        public DialogManager Dialogs { get; private set; }
        public DoodManager Doods { get; private set; }

        public void StartGame (int scene) {
            SceneManager.LoadScene(scene);

            Initialize();
        }

        private void Initialize () {
            Dialogs = new DialogManager();
            Doods = new DoodManager();

            Dialogs.Initialize();
            Doods.Initialize();
        }

        private void ShutDown () {
            Doods.ShutDown();
            Dialogs.ShutDown();

            Doods = null;
            Dialogs = null;
        }

        // todo exit a game and return to the main menu
    }

    public interface IContextManager
    {
        void Initialize ();
        void ShutDown ();
    }
}