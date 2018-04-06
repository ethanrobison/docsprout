using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menus
{
    public class OptionsMenu : Menu
    {
        private MainMenu _menu;

        public OptionsMenu (MainMenu menu) {
            _menu = menu;
        }

        public override void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(UIPrefab.OptionsMenu);

            UIUtils.FindUICompOfType<Button>(GO.transform, "Back").onClick.AddListener(Close);

            InitializeUIOptions();
        }

        private void Close () {
            _menu.CloseOptions();
        }

        private static readonly List<float> ScalingOptions = new List<float> {0.5f, 0.75f, 1f, 1.5f, 2f};

        private void InitializeUIOptions () {
            var dropdown = UIUtils.FindUICompOfType<Dropdown>(GO.transform, "UI/Scaling/Dropdown");
            dropdown.options = ScalingOptions
                .Select(f => new Dropdown.OptionData(f.ToString(CultureInfo.InvariantCulture))).ToList();
            dropdown.onValueChanged.AddListener(OnScalingChanged);
            dropdown.value = 2; // todo hard-fucking coding
        }

        // todo this is hacky and needs clean-up
        private void OnScalingChanged (int option) {
            var dropdown = UIUtils.FindUICompOfType<Dropdown>(GO.transform, "UI/Scaling/Dropdown");
            var choice = dropdown.options[option];
            var scale = float.Parse(choice.text); // todo... safety check? This seems sketch as fuck.

            Debug.Assert(0.5f <= scale && scale <= 2.0f, "Scale outside of range: " + scale);

            var scaler = UIUtils.GetCanvas().gameObject.GetComponent<CanvasScaler>();
            scaler.scaleFactor = scale;
        }

//        private void InitializeSoundOptions () {
//
//        }
    }
}