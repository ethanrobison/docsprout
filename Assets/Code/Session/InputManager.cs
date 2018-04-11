using Code.Utils;
using UnityEngine;

namespace Code.Session {
	public class InputManager : ISessionManager {

		public InputMonitor Monitor { get; private set; }

		string _platform;

		public void Initialize ()
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
				_platform = "Mac";
			} else {
				_platform = "Win";
			}

			Monitor = new InputMonitor (this);
		}

		public void ShutDown () { }

		public string GetButtonSuffix ()
		{
			string [] controllers = Input.GetJoystickNames ();
			if (controllers.Length == 0) {
				Logging.Warn ("No controllers plugged in.");
				return "";
			}

			if (controllers [0].Contains ("Xbox")) return "Xbox" + _platform;
			if (controllers [0].Contains ("Sony")) return "PS" + _platform;

			Logging.Error ("Unknown controller name: " + controllers [0]);
			return null;
		}
	}

	// todo something something standalone input module?
	public class InputMonitor {
		static string leftH = "leftHorizontal";
		static string leftV = "leftVertical";
		static string rightH = "rightHorizontal";
		static string rightV = "rightVertical";

		public float LeftH { get { return Input.GetAxisRaw (leftH); } }
		public float LeftV { get { return Input.GetAxisRaw (leftV); } }
		public float RightH { get { return Input.GetAxisRaw (rightH); } }
		public float RightV { get { return Input.GetAxisRaw (rightV); } }

		public InputMonitor (InputManager manager)
		{
			var suffix = manager.GetButtonSuffix ();
			leftH += suffix;
			leftV += suffix;
			rightH += suffix;
			rightV += suffix;
		}
	}
}
