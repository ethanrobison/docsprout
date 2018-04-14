using System;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;

namespace Code.Session {
	public enum Platform {
		Invalid, OSX, Windows, Linux
	}

	public enum Controller {
		None, XBox, Dualshock
	}

	// if you kludge long enough, it becomes the standard
	// and no one remembers the before time
	public enum ControllerButton {
		// ... shapes?
		AButton,
		BButton,
		XButton,
		YButton,

		// D-Pad
		//Up,
		//Down,
		//Left,
		//Right,

		// bumpers
		RightBumper,
		LeftBumper,

		// other
		Start,
		Select,
	}

	public class InputManager : ISessionManager {
		public InputMonitor Monitor { get; private set; }

		public Platform Platform { get; private set; }
		public Controller Controller { get; private set; }

		public void Initialize ()
		{
			SetupPlatform ();
			SetupController ();

			Monitor = Game.GO.AddComponent<InputMonitor> ();
		}

		public void ShutDown ()
		{
			UnityEngine.Object.Destroy (Monitor);
			Monitor = null;
		}


		void SetupPlatform ()
		{
			switch (Application.platform) {
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				Platform = Platform.OSX;
				break;
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				Platform = Platform.Windows;
				break;
			case RuntimePlatform.LinuxPlayer:
			case RuntimePlatform.LinuxEditor:
				Platform = Platform.Linux;
				break;
			default:
				Logging.Error ("Unknown platform: " + Application.platform);
				Platform = Platform.Invalid;
				break;
			}
		}

		void SetupController ()
		{
			string [] controllers = Input.GetJoystickNames ();

			if (controllers.Length > 0) {
				if (controllers [0].Contains ("Xbox")) {
					Controller = Controller.XBox;
					return;
				}
				if (controllers [0].Contains ("Sony")) {
					Controller = Controller.Dualshock;
					return;
				}

				Logging.Error ("Unknown controller name: " + controllers [0]);
			}

			Logging.Warn ("No controllers plugged in.");
			Controller = Controller.None;
		}
	}

	// todo something something standalone input module?
	public class InputMonitor : MonoBehaviour {
		public float LeftH { get { return Input.GetAxisRaw (leftH); } }
		public float LeftV { get { return Input.GetAxisRaw (leftV); } }
		public float RightH { get { return Input.GetAxisRaw (rightH); } }
		public float RightV { get { return Input.GetAxisRaw (rightV); } }

		string leftH;
		string leftV;
		string rightH;
		string rightV;

		Dictionary<ControllerButton, string> _buttonNames;
		readonly List<ButtonPair> _mappings = new List<ButtonPair> ();

		void Start ()
		{
			// HACK I am sorry, future me/kyle/alyssa
			leftH = GetAxisName (true, true);
			leftV = GetAxisName (true, false);
			rightH = GetAxisName (false, true);
			rightV = GetAxisName (false, false);
			SetButtonNames ();
		}

		void Update ()
		{
			foreach (var pair in _mappings) {
				if (Input.GetKeyDown (pair.ButtonName)) { pair.OnPress (); }
			}
		}


		// put all of the string manipulation nastiness into one function
		string GetAxisName (bool left, bool horizontal)
		{
			var axisname = "";
			axisname += left ? "left" : "right";
			if (Game.Sesh.Input.Controller != Controller.None) { axisname += "Joy"; }
			axisname += horizontal ? "H" : "V";

			return axisname;
		}

		void SetButtonNames ()
		{
			if (Game.Sesh.Input.Controller == Controller.None) { return; } // todo how to handle this?

			var xbox = Game.Sesh.Input.Controller == Controller.XBox;

			switch (Game.Sesh.Input.Platform) {
			case Platform.OSX:
				_buttonNames = xbox ? ButtonMappings.OSXXBox : ButtonMappings.OSXDS4;
				break;
			case Platform.Windows:
				break;
			case Platform.Linux: break;
			default:
				Logging.Error ("Invalid platform; can't choose bindings.");
				break;
			}
		}

		//
		// API

		public void RegisterMapping (ControllerButton button, Action onpress)
		{
			string buttonname;
			if (!_buttonNames.TryGetValue (button, out buttonname)) {
				Logging.Error ("Missing name for button: " + button);
				return;
			}

			var pair = new ButtonPair (buttonname, onpress);
			_mappings.Add (pair);
		}

		//
		// helper classes

		struct ButtonPair {
			public string ButtonName;
			public Action OnPress;

			public ButtonPair (string button, Action action)
			{
				ButtonName = "joystick " + button;
				OnPress = action;
			}
		}

		// I am sorry about all the hard-coding
		static class ButtonMappings {
			public static readonly Dictionary<ControllerButton, string> WindowsXBox = new Dictionary<ControllerButton, string> { };
			public static readonly Dictionary<ControllerButton, string> WindowsDS4 = new Dictionary<ControllerButton, string> { };
			public static readonly Dictionary<ControllerButton, string> OSXXBox = new Dictionary<ControllerButton, string> { };
			public static readonly Dictionary<ControllerButton, string> OSXDS4 = new Dictionary<ControllerButton, string> {
				{ControllerButton.AButton,"button 1"},
				{ControllerButton.BButton,"button 2"},
				{ControllerButton.XButton,"button 0"},
				{ControllerButton.YButton,"button 3"},
				{ControllerButton.RightBumper,"button 4"},
				{ControllerButton.LeftBumper,"button 5"},
				{ControllerButton.Start,"button 9"},
				{ControllerButton.Select,"button 8"}
			};
		}
	}
}