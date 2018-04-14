using System;
using System.Collections.Generic;
using Code;
using Code.Characters.Player;
using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOption : MonoBehaviour {
	public enum MenuOption {
		StartGame,
		LoadGame,
		Options,
		Acknowledgements,
		QuitGame
	}

	public MenuOption Option;

	MenuState _state;

	void Start ()
	{
		Action action;
		if (!Actions.TryGetValue (Option, out action)) {
			Logging.Error ("Missing action for option " + Option);
		}
		_state = new MenuState (gameObject, Option, action);
		Game.Sesh.Input.Monitor.RegisterMapping (ControllerButton.AButton, _state.PerformAction);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.GetComponent<Player> () == null) { return; }
		_state.Active = true;
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.GetComponent<Player> () == null) { return; }
		_state.Active = false;
	}

	//protected void ChangeMenuState ()
	//{
	//	if (Input.GetKeyDown (KeyCode.Q)) {
	//		_state.ChangeActiveOption (1);
	//	} else if (Input.GetKeyDown (KeyCode.E)) {
	//		_state.ChangeActiveOption (-1);
	//	}
	//}


	static void StartNewGame ()
	{
		Game.Sesh.StartGame (0);
	}

	void LoadGame () { }

	static void OpenOptions () { }

	static void QuitGame ()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
	}

	void LoadAcknowledgements () { }

	Dictionary<MenuOption, Action> Actions = new Dictionary<MenuOption, Action> {
			{ MenuOption.StartGame, StartNewGame },
			{ MenuOption.LoadGame, () => { } },
			{ MenuOption.Options, () => {} },
			{ MenuOption.Acknowledgements, () => {} },
			{ MenuOption.QuitGame, QuitGame }
	};


	//
	// container class

	class MenuState {
		MenuOption _option;
		GameObject _parent;
		GameObject _active;
		readonly Text _infoText;
		readonly Action _action;

		public bool Active {
			get { return _active.activeInHierarchy; }
			set { _active.SetActive (value); }
		}

		public MenuState (GameObject parent, MenuOption option, Action action)
		{
			_parent = parent;
			_active = _parent.transform.Find ("Active").gameObject;
			Active = false;

			_option = option;

			_action = action;

			_infoText = UIUtils.FindUICompOfType<Text> (_parent.transform, "Info/Text");
			SetText ();
		}

		//public void ChangeActiveOption (int delta)
		//{

		//}

		public void PerformAction ()
		{
			if (!Active) { return; }
			_action ();
		}

		protected void SetText ()
		{
			_infoText.text = _option.ToString ();
		}



	}
}

