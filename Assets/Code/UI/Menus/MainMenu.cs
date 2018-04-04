using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menus
{
    public class MainMenu : Menu
    {
        public override void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(UIPrefab.MainMenu);
            InitializeButtons();
        }

        private void InitializeButtons () {
            var buttons = GO.transform.Find("Buttons");

            UIUtils.FindUICompOfType<Button>(buttons, "Start").onClick.AddListener(StartNewGame);
            UIUtils.FindUICompOfType<Button>(buttons, "Load").onClick.AddListener(LoadGame);
            UIUtils.FindUICompOfType<Button>(buttons, "Options").onClick.AddListener(OpenOptions);
            UIUtils.FindUICompOfType<Button>(buttons, "Quit").onClick.AddListener(QuitGame);
            UIUtils.FindUICompOfType<Button>(GO.transform, "Acknowledgements").onClick
                .AddListener(LoadAcknowledgements);
        }

        private void StartNewGame () {
            Debug.Log("Implement me!"); // todo :3
        }

        private void LoadGame () {
            Debug.Log("Implement me!!"); // todo :3
        }

        private void OpenOptions () {
            Debug.Log("Implement me!"); // todo :3
        }

        private void QuitGame () {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
        }

        private void LoadAcknowledgements () {
            Debug.Log("Implement me!"); // todo :3
        }
    }
}