using System;
using Code.Characters.Doods;
using Code.Session;
using Code.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Characters.Player.Doodex
{
    public enum DisplayMode
    {
        Market = 0,
        SingleDood = 1,
        AllDoods = 2,
        Menu = 3,
        Max = 4,
    }

    public class Doodex
    {
        private GameObject _go;
        private bool _active;
        private Dood _activeDood;

        private DoodexTab _doodTab, _marketTab, _allDoodsTab, _menuTab;
        private DoodexTab _activeTab;
        private DisplayMode _mode = DisplayMode.Market;

        public Doodex () {
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.Start, () => {
                if (_active) { Hide(); }
                else { Show(); }
            }, menumode: MenuMode.Both);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, Hide,
                menumode: MenuMode.In);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, () => SwitchTab(1),
                menumode: MenuMode.In);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, () => SwitchTab(-1),
                menumode: MenuMode.In);
        }

        private void Show () { Show(_mode); }

        public void Show (DisplayMode mode, Dood dood = null) {
            if (_active) { return; }

            _mode = mode;

            _go = UIUtils.MakeUIPrefab(UIPrefab.Doodex);
            _activeDood = dood;
            OnShow();


            _active = true;
            _activeTab = GetAppropriateTab();
            _activeTab.Show();

            Game.Sesh.Input.Monitor.SetMenuState(true);
        }

        public void Hide () {
            if (!_active) { return; }

            Game.Sesh.Input.Monitor.SetMenuState(false);

            OnHide();
            Object.Destroy(_go);
            _go = null;

            _active = false;
        }

        private void OnShow () {
            var tabs = _go.transform.Find("Tabs");
            var tabbar = _go.transform.Find("Tab Bar");

            _doodTab = new DoodTab(tabs.Find("Dood"), tabbar.Find("Dood"), _activeDood);
            _marketTab = new MarketTab(tabs.Find("Market"), tabbar.Find("Market"));
            _allDoodsTab = new AllDoodsTab(tabs.Find("All Doods"), tabbar.Find("All Doods"));
            _menuTab = new MenuTab(tabs.Find("Menu"), tabbar.Find("Menu"));

            _doodTab.OnInitialize();
            _marketTab.OnInitialize();
            _allDoodsTab.OnInitialize();
            _menuTab.OnInitialize();
        }

        private void OnHide () {
            _doodTab.OnShutdown();
            _marketTab.OnShutdown();
            _allDoodsTab.OnShutdown();
            _menuTab.OnShutdown();
        }

        private void SwitchTab (int dir) {
            if (!_active) { return; }

            _mode += dir;
            if (_mode < 0) { _mode = DisplayMode.Max - 1; }

            if (_mode >= DisplayMode.Max) { _mode = 0; }

            _activeTab.Hide();
            _activeTab = GetAppropriateTab();
            _activeTab.Show();
        }

        private DoodexTab GetAppropriateTab () {
            switch (_mode) {
                case DisplayMode.Market: return _marketTab;
                case DisplayMode.SingleDood: return _doodTab;
                case DisplayMode.AllDoods: return _allDoodsTab;
                case DisplayMode.Menu: return _menuTab;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public abstract class DoodexTab
    {
        protected readonly GameObject GO;
        private readonly Transform _tabBar;

        protected DoodexTab (Transform tr, Transform tabbar) {
            GO = tr.gameObject;
            _tabBar = tabbar;
        }

        public abstract void OnInitialize ();
        public abstract void OnShutdown ();

        public virtual void Show () {
            _tabBar.Find("Active").gameObject.SetActive(true);
            GO.SetActive(true);
        }

        public void Hide () {
            _tabBar.Find("Active").gameObject.SetActive(false);
            GO.SetActive(false);
        }
    }
}