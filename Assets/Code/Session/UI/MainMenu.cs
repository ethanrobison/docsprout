using Code.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Code.Session.UI {
	public class MainMenu : Menu {
		static int _sceneTarget = 4; // Magic constant for the demo scene. TODO BIT OF A HACK

		protected override void CreateGameObject ()
		{
			//GO = UIUtils.MakeUIPrefab (UIPrefab.MainMenu);
			//InitializeButtons ();
		}

		void InitializeButtons ()
		{
			InitializeButton ("Buttons/Start", StartNewGame);
			Button button = UIUtils.FindUICompOfType<Button> (GO.transform, "Buttons/Start");
			GameObject.Find ("EventSystem").GetComponent<EventSystem> ().SetSelectedGameObject (button.gameObject);
			InitializeButton ("Buttons/Load", LoadGame);
			InitializeButton ("Buttons/Options", OpenOptions);
			InitializeButton ("Buttons/Quit", QuitGame);
			InitializeButton ("Acknowledgements", LoadAcknowledgements);

#if UNITY_EDITOR
			// if we are in the editor, add in the secret button to load one of the dev scenes
			var secret = GO.transform.Find ("Development");
			secret.gameObject.SetActive (true);
#endif
		}

		void InitializeButton (string name, UnityAction listener)
		{
			UIUtils.FindUICompOfType<Button> (GO.transform, name).onClick.AddListener (listener);
		}


		void StartNewGame ()
		{
#if UNITY_EDITOR
			var drop = UIUtils.FindUICompOfType<Dropdown> (GO.transform, "Development/Dropdown");
			_sceneTarget = drop.value + 1;
#endif
			Game.Sesh.StartGame (_sceneTarget);
		}

		void LoadGame ()
		{
		}

		static void OpenOptions ()
		{
			Game.Sesh.Menus.PushMenu (new OptionsMenu ());
		}

		static void QuitGame ()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
		}

		void LoadAcknowledgements ()
		{
		}
	}
}