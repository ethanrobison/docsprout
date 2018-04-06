using UnityEngine.SceneManagement;

namespace Code
{
    public class GameContext
    {
        public void StartGame (int scene) {
            SceneManager.LoadScene(scene);
        }

        private void Initialize () { }

        // todo exit a game and return to the main menu
    }
}