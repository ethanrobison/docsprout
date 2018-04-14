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
			Monitor.Initialize ();
		}

		public void ShutDown ()
		{
			Object.Destroy (Monitor);
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
}