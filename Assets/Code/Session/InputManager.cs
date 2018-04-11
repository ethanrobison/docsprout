using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Session {
	public class InputManager : ISessionManager {

		string _platform;

		public void Initialize ()
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
				_platform = "Mac";
			} else {
				_platform = "Win";
			}
		}

		public void ShutDown () { }

		public string GetButtonSuffix ()
		{
			string [] controllers = Input.GetJoystickNames ();
			if (controllers.Length == 0) {
				return "";
			}
			if (controllers [0].Contains ("Xbox")) return "Xbox" + _platform;
			if (controllers [0].Contains ("Sony")) return "PS" + _platform;
			return "";
		}

	}
}
