using System;
using System.Collections.Generic;
using Code.Environment;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Session.MainMenu
{
    public class MainMenuOption : MonoBehaviour, IApproachable
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

        private MenuInfo _state;

        private void Start () {
            MakeState();
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, () => {
                if (_state.Active) { _state.ChangeOption(1); }
            });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, () => {
                if (_state.Active) { _state.ChangeOption(-1); }
            });
        }

        private void MakeState () {
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

        public void OnApproach () { _state.SetState(true); }
        public void OnDepart () { _state.SetState(false); }
        public void Interact () { _state.PerformAction(); }

        //
        // menu state

        private abstract class MenuInfo
        {
            public bool Active { get; private set; }

            protected int CurrentOption;

            protected readonly Text InfoText;
            protected readonly List<MenuOption> Options = new List<MenuOption>();

            private readonly SineWiggle _wiggle;

            protected MenuInfo (GameObject parent, OptionType option) {
                var active = parent.transform.Find("Active").gameObject;
                _wiggle = active.GetRequiredComponentInChildren<SineWiggle>();

                InfoText = UIUtils.FindUICompOfType<Text>(parent.transform, "Info/Text");
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

            public void SetState (bool active) {
                Active = active;
                _wiggle.Magnitude = active ? 10f : 0f;
            }

            public void Reset () {
                SetState(false);
                SetText();
            }
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