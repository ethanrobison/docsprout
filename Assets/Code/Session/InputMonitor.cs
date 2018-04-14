using System;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;

namespace Code.Session {
	/// <summary>
	/// Unwieldy class for checking for button presses. I am sorry about all of the hard-coding.
	/// </summary>
	public class InputMonitor : MonoBehaviour, ISessionManager {
		public float LeftH { get { return Input.GetAxisRaw (leftH); } }
		public float LeftV { get { return Input.GetAxisRaw (leftV); } }
		public float RightH { get { return Input.GetAxisRaw (rightH); } }
		public float RightV { get { return Input.GetAxisRaw (rightV); } }

		string leftH;
		string leftV;
		string rightH;
		string rightV;

		Dictionary<ControllerButton, KeyCode> _buttonNames;
		readonly List<ButtonPair> _mappings = new List<ButtonPair> ();

		public void Initialize ()
		{
			_mappings.Clear ();

			// HACK I am sorry, future me/kyle/alyssa
			leftH = GetAxisName (true, true);
			leftV = GetAxisName (true, false);
			rightH = GetAxisName (false, true);
			rightV = GetAxisName (false, false);
			SetButtonNames ();
		}

		public void ShutDown () { }

		public void OnGameStart() {
			_mappings.Clear ();
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
			KeyCode buttonname;
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
			public KeyCode ButtonName;
			public Action OnPress;

			public ButtonPair (KeyCode button, Action action)
			{
				ButtonName = button;
				OnPress = action;
			}
		}

		// I am sorry about all the hard-coding
		static class ButtonMappings {
			//public static readonly Dictionary<ControllerButton, string> WindowsXBox = new Dictionary<ControllerButton, string> { };
			//public static readonly Dictionary<ControllerButton, string> WindowsDS4 = new Dictionary<ControllerButton, string> { };
			public static readonly Dictionary<ControllerButton, KeyCode> OSXXBox = new Dictionary<ControllerButton, KeyCode> {
				{ControllerButton.AButton,KeyCode.JoystickButton16},
				{ControllerButton.BButton,KeyCode.JoystickButton17},
				{ControllerButton.XButton, KeyCode.JoystickButton18},
				{ControllerButton.YButton,KeyCode.JoystickButton19},
				{ControllerButton.RightBumper,KeyCode.JoystickButton13},
				{ControllerButton.LeftBumper, KeyCode.JoystickButton14},
				{ControllerButton.Start,KeyCode.JoystickButton9},
				{ControllerButton.Select,KeyCode.JoystickButton10}
			};
			public static readonly Dictionary<ControllerButton, KeyCode> OSXDS4 = new Dictionary<ControllerButton, KeyCode> {
				{ControllerButton.AButton,KeyCode.JoystickButton1},
				{ControllerButton.BButton,KeyCode.JoystickButton2},
				{ControllerButton.XButton, KeyCode.JoystickButton0},
				{ControllerButton.YButton,KeyCode.JoystickButton3},
				{ControllerButton.RightBumper,KeyCode.JoystickButton4},
				{ControllerButton.LeftBumper, KeyCode.JoystickButton5},
				{ControllerButton.Start,KeyCode.JoystickButton9},
				{ControllerButton.Select,KeyCode.JoystickButton17}
			};
		}
	}
}