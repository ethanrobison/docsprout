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

	MenuInfo _state;

	void Start ()
	{
		MakeState ();
		var monitor = Game.Sesh.Input.Monitor;
		monitor.RegisterMapping (ControllerButton.AButton, () => {
			if (_state.Active) { _state.PerformAction (); }
		});
		monitor.RegisterMapping (ControllerButton.RightBumper, () => {
			if (_state.Active) { _state.ChangeOption (1); }
		});
		monitor.RegisterMapping (ControllerButton.LeftBumper, () => {
			if (_state.Active) { _state.ChangeOption (-1); }
		});
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
		_state.Reset ();
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

	abstract class MenuInfo {
		MenuOption _option;
		GameObject _parent, _active;
		readonly protected Text _infoText;

		public bool Active {
			get { return _active.activeInHierarchy; }
			set { _active.SetActive (value); }
		}

		protected MenuInfo (GameObject parent, MenuOption option)
		{
			_parent = parent;
			_active = _parent.transform.Find ("Active").gameObject;
			Active = false;

			_option = option;

			_infoText = UIUtils.FindUICompOfType<Text> (_parent.transform, "Info/Text");
			_infoText.text = _option.ToString ();
		}

		protected virtual void SetText () { }

		public abstract void PerformAction ();
		public abstract void ChangeOption (int direction);
		public void Reset ()
		{
			SetText ();
		}

	}


	//
	// Menu state

	class QuitState : MenuInfo {
		public QuitState (GameObject parent, MenuOption option = MenuOption.QuitGame) : base (parent, option) { }

		public override void PerformAction ()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
		}
		public override void ChangeOption (int direction) { }
	}

	class StartState : MenuInfo {
		int _scene = 1;

		public StartState (GameObject parent, MenuOption option = MenuOption.StartGame) : base (parent, option) { }

		// FIXME I am hard-coded
		public override void PerformAction ()
		{
#if UNITY_EDITOR
			Game.Sesh.StartGame (_scene);
#else
			Game.Sesh.StartGame (3);
#endif
		}

		const int SCENE_COUNT = 4;
		public override void ChangeOption (int direction)
		{
			_scene += direction;
			if (_scene > SCENE_COUNT) { _scene -= SCENE_COUNT; }
			if (_scene < 1) { _scene += SCENE_COUNT; }
			SetText ();
		}
		protected override void SetText ()
		{
			_infoText.text = string.Format ("Start scene: {0}", _scene);
		}
	}


	class OptionsState : MenuInfo {
		public OptionsState (GameObject parent, MenuOption option = MenuOption.Options) : base (parent, option) { }

		public override void PerformAction () { }
		public override void ChangeOption (int direction) { }
	}


	class AcknowledgementsState : MenuInfo {
		public AcknowledgementsState (GameObject parent, MenuOption option = MenuOption.Acknowledgements) : base (parent, option) { }

		public override void PerformAction () { }
		public override void ChangeOption (int direction) { }
	}


	class LoadState : MenuInfo {
		public LoadState (GameObject parent, MenuOption option = MenuOption.LoadGame) : base (parent, option) { }

		public override void PerformAction () { }
		public override void ChangeOption (int direction) { }
	}
}

