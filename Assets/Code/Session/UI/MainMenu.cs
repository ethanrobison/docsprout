using Code.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Session.UI
{
    public class MainMenu : Menu
    {
        private static int _sceneTarget = 4; // Magic constant for the demo scene. TODO BIT OF A HACK

        protected override void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(UIPrefab.MainMenu);
            InitializeButtons();
        }

        private void InitializeButtons () {
            InitializeButton("Buttons/Start", StartNewGame);
            InitializeButton("Buttons/Load", LoadGame);
            InitializeButton("Buttons/Options", OpenOptions);
            InitializeButton("Buttons/Quit", QuitGame);
            InitializeButton("Acknowledgements", LoadAcknowledgements);

#if UNITY_EDITOR
            // if we are in the editor, add in the secret button to load one of the dev scenes
            var secret = GO.transform.Find("Development");
            secret.gameObject.SetActive(true);
#endif
        }

        private void InitializeButton (string name, UnityAction listener) {
            UIUtils.FindUICompOfType<Button>(GO.transform, name).onClick.AddListener(listener);
        }


        private void StartNewGame () {
#if UNITY_EDITOR
            var drop = UIUtils.FindUICompOfType<Dropdown>(GO.transform, "Development/Dropdown");
            _sceneTarget = drop.value + 1;
#endif
            Game.Sesh.StartGame(_sceneTarget);
        }

        private void LoadGame () {
            Debug.Log("Implement me!!"); // todo :3
        }

        private static void OpenOptions () {
            Game.Sesh.Menus.PushMenu(new OptionsMenu());
        }

        private static void QuitGame () {
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