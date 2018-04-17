using System.Collections.Generic;
using Code.Utils;
using UnityEngine.UI;

namespace Code.Session
{
    public class PreferencesManager : ISessionManager
    {
        public UISettings UI { get; private set; }

        public void Initialize () {
            UI = new UISettings();

            UI.Initialize();
        }

        public void ShutDown () {
            UI.ShutDown();

            UI = null;
        }

        public void OnGameStart () { }
    }

    public class UISettings : ISessionManager
    {
        public readonly List<float> UIScalingList = new List<float> {0.5f, 0.75f, 1f, 1.5f, 2f};

        public void Initialize () { }

        public void ShutDown () { }

        public void OnGameStart () { }

        public void SetUIScale (int option) {
            var scale = UIScalingList[option];
            var scaler = UIUtils.GetCanvas().gameObject.GetComponent<CanvasScaler>();
            scaler.scaleFactor = scale;
        }
    }
}