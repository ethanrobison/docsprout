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
        protected override void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(UIPrefab.OptionsMenu);

            UIUtils.FindUICompOfType<Button>(GO.transform, "Back").onClick.AddListener(Close);

            InitializeUIOptions();
        }

        private void Close () {
            Game.Sesh.Menus.CloseToMe(this);
        }

        private void InitializeUIOptions () {
            var dropdown = UIUtils.FindUICompOfType<Dropdown>(GO.transform, "UI/Scaling/Dropdown");
            dropdown.options = Game.Sesh.Prefs.UI.UIScalingList
                .Select(f => new Dropdown.OptionData(f.ToString(CultureInfo.InvariantCulture))).ToList();
            dropdown.onValueChanged.AddListener(OnScalingChanged);
            dropdown.value = 2; // todo hard-fucking coding
        }

        private static void OnScalingChanged (int option) {
            Game.Sesh.Prefs.UI.SetUIScale(option);
        }

//        private void InitializeSoundOptions () {
//
//        }
    }
}