using Code.Utils;
using UnityEngine;

namespace Code.Session {
	public enum Platform {
		Invalid, OSX, Windows, Linux
	}

	public enum Controller {
		None, XBox, Dualshock
	}

	public class InputManager : ISessionManager {
		public InputMonitor Monitor { get; private set; }

		public Platform Platform { get; private set; }
		public Controller Controller { get; private set; }

		public void Initialize ()
		{
			SetupPlatform ();
			SetupController ();

			Monitor = new InputMonitor ();
		}

		public void ShutDown () { }

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
	public class InputMonitor {
		readonly string leftH;
		readonly string leftV;
		readonly string rightH;
		readonly string rightV;

		public float LeftH { get { return Input.GetAxisRaw (leftH); } }
		public float LeftV { get { return Input.GetAxisRaw (leftV); } }
		public float RightH { get { return Input.GetAxisRaw (rightH); } }
		public float RightV { get { return Input.GetAxisRaw (rightV); } }

		public InputMonitor ()
		{
			leftH = GetAxisName (true, true);
			leftV = GetAxisName (true, false);
			rightH = GetAxisName (false, true);
			rightV = GetAxisName (false, false);
		}

		// put all of the string manipulation nastiness into one function
		string GetAxisName (bool left, bool horizontal)
		{
			string name = "";
			name += left ? "left" : "right";
			if (Game.Sesh.Input.Controller != Controller.None) { name += "Joy"; }
			name += horizontal ? "H" : "V";

			return name;
		}
	}
}
