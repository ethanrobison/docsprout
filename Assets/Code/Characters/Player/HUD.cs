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
        private readonly Text _info;
        private Inventory _inventory;

        public HUD () {
            var go = UIUtils.MakeUIPrefab(UIPrefab.HUD);
            _info = UIUtils.FindUICompOfType<Text>(go, "Info/Text");
            Game.Ctx.Economy.Stats.OnFrootChanged += OnFrootChanged;

            _inventory = new Inventory();
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, OnLeftPress);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, OnRightPress);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.XButton, OnDownPress);
        }

        private void OnFrootChanged (int froot) { _info.text = string.Format("{0}", froot); }

        private void OnLeftPress () { ChangeActiveItem(-1); }

        private void OnRightPress () { ChangeActiveItem(1); }

        private void OnDownPress () { _inventory.UseItem(); }

        private void ChangeActiveItem (int delta) {
            Logging.Assert(Mathf.Abs(delta) <= 1, "Invalid delta.");
            _inventory.ChangeActive(delta);
        }


        private class Inventory
        {
            private int _active;
            private readonly List<ItemInfo> _items = new List<ItemInfo>();

            public void ChangeActive (int delta) {
                _active += delta;
                if (_active < 0) { _active = _items.Count - 1; }
                else if (_active > _items.Count) { _active = 0; }

                Logging.Log(_active);
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
            public int Count { get; private set; }

            public string GetInfo () { return ""; }

            public void Use () { }

            public void Add (ItemInfo item) { Count++; }
        }
    }
}