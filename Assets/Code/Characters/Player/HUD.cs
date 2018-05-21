using System;
using System.Collections.Generic;
using System.Linq;
using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Characters.Player
{
    public class HUDManager : IContextManager
    {
        public HUD HUD { get; private set; }

        public void Initialize () {
            HUD = new HUD();
            Game.Ctx.Economy.Stats.SpendFroot(0);
        }

        public void ShutDown () { }
    }

    public class HUD
    {
        private readonly GameObject _go;
        private RectTransform _activeIcon;
        private readonly Text _info;
        private readonly Inventory _inventory;

        public HUD () {
            _go = UIUtils.MakeUIPrefab(UIPrefab.HUD);
            _info = UIUtils.FindUICompOfType<Text>(_go, "Info/Text");
            Game.Ctx.Economy.Stats.OnFrootChanged += OnFrootChanged;

            _inventory = new Inventory();

            MapActionToButton(_go, "Buttons/Left", ControllerButton.LeftBumper, OnLeftPress);
            MapActionToButton(_go, "Buttons/Right", ControllerButton.RightBumper, OnRightPress);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.XButton, UseItem);

            SetActiveIcon(0);
        }

        private static void MapActionToButton (GameObject go, string name, ControllerButton button, Action onpress) {
            var btn = UIUtils.FindUICompOfType<Button>(go, name);
            btn.onClick.AddListener(() => onpress());
            Game.Sesh.Input.Monitor.RegisterMapping(button, () => btn.OnSubmit(null));
        }

        private void OnFrootChanged (int froot) { _info.text = string.Format("{0}", froot); }

        private void OnLeftPress () { ChangeActiveItem(-1); }

        private void OnRightPress () { ChangeActiveItem(1); }

        private void ChangeActiveItem (int delta) {
            Logging.Assert(Mathf.Abs(delta) <= 1, "Invalid delta.");
            var active = _inventory.ChangeActive(delta);
            SetActiveIcon(active);
        }

        // todo fix magic constants
        private void SetActiveIcon (int active) {
            var icons = _go.transform.Find("Buttons/Icons");
            if (_activeIcon != null) {
                _activeIcon.localScale = Vector3.one;
                _activeIcon.GetComponent<Button>().interactable = false;
            }

            _activeIcon = icons.GetChild(active).GetComponent<RectTransform>();
            _activeIcon.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _activeIcon.GetComponent<Button>().interactable = true;
        }

        private void UseItem () {
            _inventory.UseItem();
            _activeIcon.GetComponent<Button>().OnSubmit(null);
        }


        private class Inventory
        {
            private int _active;

            private readonly List<ItemInfo> _items = new List<ItemInfo> {
                new ItemInfo { Name = "Donut" },
                new ItemInfo { Name = "Fertilizer" },
                new ItemInfo { Name = "Toast" },
            };

            public int ChangeActive (int delta) {
                _active += delta;
                if (_active < 0) { _active = _items.Count - 1; }
                else if (_active >= _items.Count) { _active = 0; }

                return _active;
            }

            public void UseItem () {
                Logging.Log(_active);
                _items[_active].Use();
            }

            // todo shite
            public void AddItem (ItemInfo item) {
                if (_items.Contains(item)) { _items.First(i => i == item).Add(item); }
                else { _items.Add(item); }
            }
        }

        private class ItemInfo
        {
            public string Name;

            public int Count { get; private set; }

            public void Use () { }

            public void Add (ItemInfo item) { Count++; }
        }
    }
}