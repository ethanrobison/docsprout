using System;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;

namespace Code.Session
{
    /// <summary>
    /// Unwieldy class for checking for button presses. I am sorry about all of the hard-coding.
    /// </summary>
    public class InputMonitor : MonoBehaviour, ISessionManager
    {
        public enum PressType
        {
            ButtonDown,
            ButtonUp,
            Hold
        }

        public float LeftH {
            get { return Input.GetAxisRaw(_leftH); }
        }

        public float LeftV {
            get { return Input.GetAxisRaw(_leftV); }
        }

        public float RightH {
            get { return Input.GetAxisRaw(_rightH); }
        }

        public float RightV {
            get { return Input.GetAxisRaw(_rightV); }
        }

        public float RT {
            get { return Input.GetAxisRaw(_RT); }
        }

        public float LT {
            get { return Input.GetAxisRaw(_LT); }
        }

        private string _leftH;
        private string _leftV;
        private string _rightH;
        private string _rightV;
        private string _RT = "RT";
        private string _LT = "LT";

        private Dictionary<ControllerButton, KeyCode> _buttonNames;
        private readonly List<ButtonPair> _mappings = new List<ButtonPair>();

        public void Initialize () {
            // HACK I am sorry, future me/kyle/alyssa
            _leftH = GetAxisName(true, true);
            _leftV = GetAxisName(true, false);
            _rightH = GetAxisName(false, true);
            _rightV = GetAxisName(false, false);
            SetButtonNames();
        }

        public void ShutDown () { }

        public void OnGameStart () { _mappings.Clear(); }

        // todo maybe loop unroll me? seems like overkill
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

        private void SetButtonNames () {
            if (Game.Sesh.Input.Controller == Controller.None) {
                return;
            } // todo how to handle this?

            var xbox = Game.Sesh.Input.Controller == Controller.XBox;

            switch (Game.Sesh.Input.Platform) {
                case Platform.OSX:
                    _buttonNames = xbox ? ButtonMappings.OSXXBox : ButtonMappings.OSXDS4;
                    break;
                case Platform.Windows:
                    break;
                case Platform.Linux:
                    break;
                case Platform.Invalid:
                    Logging.Error("Invalid platform; can't choose bindings.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //
        // API

        public void RegisterMapping (ControllerButton button, Action onpress, PressType type = PressType.ButtonDown) {
            KeyCode buttonname;
            if (!_buttonNames.TryGetValue(button, out buttonname)) {
                Logging.Error("Missing name for button: " + button);
                return;
            }

            var pair = new ButtonPair(buttonname, onpress, type);
            _mappings.Add(pair);
        }

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

        // I am sorry about all the hard-coding
        private static class ButtonMappings
        {
            //public static readonly Dictionary<ControllerButton, string> WindowsXBox = new Dictionary<ControllerButton, string> { };
            //public static readonly Dictionary<ControllerButton, string> WindowsDS4 = new Dictionary<ControllerButton, string> { };
            public static readonly Dictionary<ControllerButton, KeyCode> OSXXBox =
                new Dictionary<ControllerButton, KeyCode> {
                    { ControllerButton.AButton, KeyCode.JoystickButton16 },
                    { ControllerButton.BButton, KeyCode.JoystickButton17 },
                    { ControllerButton.XButton, KeyCode.JoystickButton18 },
                    { ControllerButton.YButton, KeyCode.JoystickButton19 },
                    { ControllerButton.RightBumper, KeyCode.JoystickButton14 },
                    { ControllerButton.LeftBumper, KeyCode.JoystickButton13 },
                    { ControllerButton.Start, KeyCode.JoystickButton9 },
                    { ControllerButton.Select, KeyCode.JoystickButton10 }
                };

            public static readonly Dictionary<ControllerButton, KeyCode> OSXDS4 =
                new Dictionary<ControllerButton, KeyCode> {
                    { ControllerButton.AButton, KeyCode.JoystickButton1 },
                    { ControllerButton.BButton, KeyCode.JoystickButton2 },
                    { ControllerButton.XButton, KeyCode.JoystickButton0 },
                    { ControllerButton.YButton, KeyCode.JoystickButton3 },
                    { ControllerButton.RightBumper, KeyCode.JoystickButton5 },
                    { ControllerButton.LeftBumper, KeyCode.JoystickButton4 },
                    { ControllerButton.Start, KeyCode.JoystickButton9 },
                    { ControllerButton.Select, KeyCode.JoystickButton17 }
                };
        }
    }
}