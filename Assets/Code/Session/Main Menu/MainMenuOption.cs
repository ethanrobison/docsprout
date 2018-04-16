using System.Collections.Generic;
using Code;
using Code.Characters.Player;
using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOption : MonoBehaviour {
	public enum OptionType {
		StartGame,
		LoadGame,
		Options,
		Acknowledgements,
		QuitGame
	}

	public OptionType Option;

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
		case OptionType.StartGame:
			_state = new StartState (gameObject);
			break;
		case OptionType.LoadGame:
			_state = new LoadState (gameObject);
			break;
		case OptionType.Options:
			_state = new OptionsState (gameObject);
			break;
		case OptionType.Acknowledgements:
			_state = new AcknowledgementsState (gameObject);
			break;
		case OptionType.QuitGame:
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
	// menu state

	abstract class MenuInfo {
		OptionType _option;
		GameObject _parent, _active;
		protected int _currentOption;

		readonly protected Text _infoText;
		readonly protected List<MenuOption> _options = new List<MenuOption> ();

		public bool Active {
			get { return _active != null && _active.activeInHierarchy; }
			set { _active.SetActive (value); }
		}

		protected MenuInfo (GameObject parent, OptionType option)
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
		public virtual void ChangeOption (int direction)
		{
			var max = _options.Count;
			_currentOption += direction;
			if (_currentOption < 0) { _currentOption += max; }
			if (_currentOption > max - 1) { _currentOption -= max; }
		}

		public void Reset ()
		{
			SetText ();
		}

	}

	struct MenuOption {
		public int Index { get; private set; }
		public string Description { get; private set; }

		public MenuOption (string description, int index)
		{
			Index = index;
			Description = description;
		}
	}

	class QuitState : MenuInfo {
		public QuitState (GameObject parent, OptionType option = OptionType.QuitGame) : base (parent, option) { }

		public override void PerformAction ()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
		}
	}

	class StartState : MenuInfo {
		public StartState (GameObject parent, OptionType option = OptionType.StartGame) : base (parent, option)
		{
			_options.Add (new MenuOption ("dev-ethan", 1));
			_options.Add (new MenuOption ("dev-kyle", 2));
			_options.Add (new MenuOption ("dev-alyssa", 3));
			_options.Add (new MenuOption ("Demo", 4));
		}

		// FIXME I am hard-coded
		public override void PerformAction ()
		{
#if UNITY_EDITOR
			Game.Sesh.StartGame (_options [_currentOption].Index);
#else
			Game.Sesh.StartGame (3);
#endif
		}

		public override void ChangeOption (int direction)
		{
			base.ChangeOption (direction);
			SetText ();
		}

		protected override void SetText ()
		{
			_infoText.text = string.Format ("Start scene: {0}", _options [_currentOption].Index);
		}
	}


	class OptionsState : MenuInfo {
		public OptionsState (GameObject parent, OptionType option = OptionType.Options) : base (parent, option) { }

		public override void PerformAction () { }
	}


	class AcknowledgementsState : MenuInfo {
		public AcknowledgementsState (GameObject parent, OptionType option = OptionType.Acknowledgements) : base (parent, option) { }

		public override void PerformAction () { }
	}


	class LoadState : MenuInfo {
		public LoadState (GameObject parent, OptionType option = OptionType.LoadGame) : base (parent, option) { }

		public override void PerformAction () { }
	}
}

