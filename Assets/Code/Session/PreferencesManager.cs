using System.Collections.Generic;
using Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Session
{
    public class PreferencesManager : ISessionManager
    {
        public UISettings UI { get; private set; }

        public void Initialize () {
            UI = new UISettings();
        }

        public void ShutDown () { }
    }

    public class UISettings
    {
        public readonly List<float> UIScalingList = new List<float> {0.5f, 0.75f, 1f, 1.5f, 2f};

        public void SetUIScale (int option) {
            var scale = UIScalingList[option];
            var scaler = UIUtils.GetCanvas().gameObject.GetComponent<CanvasScaler>();
            scaler.scaleFactor = scale;
        }
    }
}