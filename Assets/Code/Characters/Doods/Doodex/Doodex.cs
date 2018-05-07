using System;
using Code.Characters.Doods.Needs;
using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.Characters.Doods.Doodex
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
        private DisplayMode _mode;

        public Doodex () {
            return; // todo fix this noop; doodex is broken :(
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.Start, () => { Show(DisplayMode.Menu); });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, Hide);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, () => SwitchTab(1));
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, () => SwitchTab(-1));
        }

        public void Show (DisplayMode mode, Dood dood = null) {
            if (_active) { return; }

            _mode = mode;

            _go = UIUtils.MakeUIPrefab(UIPrefab.Doodex);
            _activeDood = dood;
            OnShow();


            _active = true;
            _activeTab = GetAppropriateTab();
            _activeTab.Show();
        }

        private void Hide () {
            if (!_active) { return; }

            OnHide();
            Object.Destroy(_go);
            _go = null;

            _active = false;
        }

        private void OnShow () {
            var tabs = _go.transform.Find("Tabs");
            var tabbar = _go.transform.Find("Tab Bar");

            _doodTab = new DoodTab(tabs.Find("Dood"), tabbar.Find("Dood"), _activeDood);
            _doodTab.OnInitialize();

            _marketTab = new MarketTab(tabs.Find("Market"), tabbar.Find("Market"));
            _marketTab.OnInitialize();

            _allDoodsTab = new AllDoodsTab(tabs.Find("All Doods"), tabbar.Find("All Doods"));
            _allDoodsTab.OnInitialize();

            _menuTab = new MenuTab(tabs.Find("Menu"), tabbar.Find("Menu"));
            _menuTab.OnInitialize();
        }

        private void OnHide () {
            _doodTab.OnShutdown();
//            _market.OnShutdown();
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

        // todo generative tabs?
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