using System;
using System.Collections.Generic;
using Code.Characters.Player;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Session.MainMenu
{
    public class MainMenuOption : MonoBehaviour
    {
        public enum OptionType
        {
            StartGame,
            LoadGame,
            Options,
            Acknowledgements,
            QuitGame
        }

        public OptionType Option;

        MenuInfo _state;

        private void Start () {
            MakeState();
            var monitor = Game.Sesh.Input.Monitor;
            monitor.RegisterMapping(ControllerButton.AButton, () => {
                if (_state.Active) { _state.PerformAction(); }
            });
            monitor.RegisterMapping(ControllerButton.RightBumper, () => {
                if (_state.Active) { _state.ChangeOption(1); }
            });
            monitor.RegisterMapping(ControllerButton.LeftBumper, () => {
                if (_state.Active) { _state.ChangeOption(-1); }
            });
        }

        void MakeState () {
            switch (Option) {
                case OptionType.StartGame:
                    _state = new StartState(gameObject);
                    break;
                case OptionType.LoadGame:
                    _state = new LoadState(gameObject);
                    break;
                case OptionType.Options:
                    _state = new OptionsState(gameObject);
                    break;
                case OptionType.Acknowledgements:
                    _state = new AcknowledgementsState(gameObject);
                    break;
                case OptionType.QuitGame:
                    _state = new QuitState(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _state.Reset();
        }

        private void OnTriggerEnter (Collider other) {
            if (other.gameObject.GetComponent<Player>() == null) { return; }

            _state.Active = true;
        }

        private void OnTriggerExit (Collider other) {
            if (other.gameObject.GetComponent<Player>() == null) { return; }

            _state.Active = false;
        }


        //
        // menu state

        private abstract class MenuInfo
        {
            protected int CurrentOption;

            protected readonly Text InfoText;
            protected readonly List<MenuOption> Options = new List<MenuOption>();

            private readonly GameObject _active;

            public bool Active {
                get { return _active != null && _active.activeInHierarchy; }
                set { _active.SetActive(value); }
            }

            protected MenuInfo (GameObject parent, OptionType option) {
                var parent1 = parent;
                _active = parent1.transform.Find("Active").gameObject;
                Active = false;

                InfoText = UIUtils.FindUICompOfType<Text>(parent1.transform, "Info/Text");
                InfoText.text = option.ToString();
            }

            protected virtual void SetText () { }

            public abstract void PerformAction ();

            public virtual void ChangeOption (int direction) {
                var max = Options.Count;
                CurrentOption += direction;
                if (CurrentOption < 0) { CurrentOption += max; }

                if (CurrentOption > max - 1) { CurrentOption -= max; }
            }

            public void Reset () { SetText(); }
        }

        private struct MenuOption
        {
            public int Index { get; private set; }

            public MenuOption (int index) : this() { Index = index; }
        }

        private class QuitState : MenuInfo
        {
            public QuitState (GameObject parent, OptionType option = OptionType.QuitGame) : base(parent, option) { }

            public override void PerformAction () {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
            }
        }

        private class StartState : MenuInfo
        {
            public StartState (GameObject parent, OptionType option = OptionType.StartGame) : base(parent, option) {
                Options.Add(new MenuOption(1));
                Options.Add(new MenuOption(2));
                Options.Add(new MenuOption(3));
                Options.Add(new MenuOption(4));
            }

            // FIXME I am hard-coded
            public override void PerformAction () {
#if UNITY_EDITOR
                Game.Sesh.StartGame(Options[CurrentOption].Index);
#else
			Game.Sesh.StartGame (3);
#endif
            }

            public override void ChangeOption (int direction) {
                base.ChangeOption(direction);
                SetText();
            }

            protected override void SetText () {
                InfoText.text = string.Format("Start scene: {0}", Options[CurrentOption].Index);
            }
        }


        private class OptionsState : MenuInfo
        {
            public OptionsState (GameObject parent, OptionType option = OptionType.Options) : base(parent, option) { }

            public override void PerformAction () { }
        }


        private class AcknowledgementsState : MenuInfo
        {
            public AcknowledgementsState (GameObject parent, OptionType option = OptionType.Acknowledgements) : base(
                parent, option) { }

            public override void PerformAction () { }
        }


        private class LoadState : MenuInfo
        {
            public LoadState (GameObject parent, OptionType option = OptionType.LoadGame) : base(parent, option) { }

            public override void PerformAction () { }
        }
    }
}