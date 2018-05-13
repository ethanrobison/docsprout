using System;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;

namespace Code.Session
{
    /// <summary>
    /// Unwieldy class for checking for button presses. I am sorry about all of the hard-coding.
    /// </summary>
    public enum PressType
    {
        None,
        ButtonDown,
        ButtonUp,
        Hold
    }

    public class InputMonitor : MonoBehaviour, ISessionManager
    {
        public float LeftH {
            get { return _inMenu ? 0f : Input.GetAxisRaw(_leftH); }
        }

        public float LeftV {
            get { return _inMenu ? 0f : Input.GetAxisRaw(_leftV); }
        }

        public float RightH {
            get { return _inMenu ? 0f : Input.GetAxisRaw(_rightH); }
        }

        public float RightV {
            get { return _inMenu ? 0f : Input.GetAxisRaw(_rightV); }
        }

        private string _leftH;
        private string _leftV;
        private string _rightH;
        private string _rightV;

        private bool _inMenu;

        private readonly List<ButtonPair> _mappings = new List<ButtonPair>();

        public void Initialize () {
            // HACK I am sorry, future me/kyle/alyssa
            _leftH = GetAxisName(true, true);
            _leftV = GetAxisName(true, false);
            _rightH = GetAxisName(false, true);
            _rightV = GetAxisName(false, false);
        }

        public void ShutDown () { }

        public void OnGameStart () { _mappings.Clear(); }

        private void Update () {
            for (int i = 0, c = _mappings.Count; i < c; i++) {
                var pair = _mappings[i];
                switch (pair.PressType) {
                    case PressType.ButtonDown:
                        if (Input.GetKeyDown(pair.ButtonName)) { pair.OnPress(); }

                        break;
                    case PressType.ButtonUp:
                        if (Input.GetKeyUp(pair.ButtonName)) { pair.OnPress(); }

                        break;
                    case PressType.Hold:
                        if (Input.GetKey(pair.ButtonName)) { pair.OnPress(); }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                c = _mappings.Count; // hack to prevent errors on scene loading
            }
        }


        // put all of the string manipulation nastiness into one function
        private static string GetAxisName (bool left, bool horizontal) {
            var axisname = "";
            axisname += left ? "left" : "right";
            if (Game.Sesh.Input.Controller != Controller.None) {
                axisname += "Joy";
            }

            axisname += horizontal ? "H" : "V";

            return axisname;
        }

        //
        // API

        public void RegisterMapping (ControllerButton button, Action onpress, PressType type = PressType.ButtonDown) {
            KeyCode buttonname;
            if (!Game.Sesh.Input.ButtonNames.TryGetValue(button, out buttonname)) {
                Logging.Error("Missing name for button: " + button);
                return;
            }

            var pair = new ButtonPair(buttonname, onpress, type);
            _mappings.Add(pair);
        }

        public void SetMenuState (bool state) { _inMenu = state; }

        //
        // helper classes

        private struct ButtonPair
        {
            public readonly KeyCode ButtonName;

            public readonly Action OnPress;
            public readonly PressType PressType;

            public ButtonPair (KeyCode button, Action action, PressType type) {
                ButtonName = button;
                OnPress = action;
                PressType = type;
            }
        }
    }
}