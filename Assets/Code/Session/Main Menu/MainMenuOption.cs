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
		MakeState ();
		var monitor = Game.Sesh.Input.Monitor;
		monitor.RegisterMapping (ControllerButton.AButton, () => {
			if (_state.Active) { _state.PerformAction (); }
		});
		//monitor.RegisterMapping(ControllerButton.RightBumper)
	}

	void MakeState ()
	{
		switch (Option) {
		case MenuOption.StartGame:
			_state = new StartState (gameObject);
			break;
		case MenuOption.LoadGame:
			_state = new LoadState (gameObject);
			break;
		case MenuOption.Options:
			_state = new OptionsState (gameObject);
			break;
		case MenuOption.Acknowledgements:
			_state = new AcknowledgementsState (gameObject);
			break;
		case MenuOption.QuitGame:
			_state = new QuitState (gameObject);
			break;
		}
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


	//
	// container class

	abstract class MenuState {
		MenuOption _option;
		GameObject _parent, _active;
		readonly Text _infoText;

		public bool Active {
			get { return _active.activeInHierarchy; }
			set { _active.SetActive (value); }
		}

		protected MenuState (GameObject parent, MenuOption option)
		{
			_parent = parent;
			_active = _parent.transform.Find ("Active").gameObject;
			Active = false;

			_option = option;

			_infoText = UIUtils.FindUICompOfType<Text> (_parent.transform, "Info/Text");
			SetText ();
		}

		public abstract void PerformAction ();

		protected void SetText ()
		{
			_infoText.text = _option.ToString ();
		}
	}


	//
	// Menu state

	class QuitState : MenuState {
		public QuitState (GameObject parent, MenuOption option = MenuOption.QuitGame) : base (parent, option) { }

		public override void PerformAction ()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
		}
	}

	class StartState : MenuState {
		public StartState (GameObject parent, MenuOption option = MenuOption.StartGame) : base (parent, option) { }
		public override void PerformAction ()
		{
			Game.Sesh.StartGame (1);
		}
	}

	class OptionsState : MenuState {
		public OptionsState (GameObject parent, MenuOption option = MenuOption.Options) : base (parent, option) { }

		public override void PerformAction () { }
	}

	class AcknowledgementsState : MenuState {
		public AcknowledgementsState (GameObject parent, MenuOption option = MenuOption.Acknowledgements) : base (parent, option) { }

		public override void PerformAction () { }
	}

	class LoadState : MenuState {
		public LoadState (GameObject parent, MenuOption option = MenuOption.LoadGame) : base (parent, option) { }

		public override void PerformAction () { }
	}
}

