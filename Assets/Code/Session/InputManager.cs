using System;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Code.Session
{
    public enum Platform
    {
        Invalid,
        OSX,
        Windows,
        Linux
    }

    public enum Controller
    {
        None,
        XBox,
        Dualshock
    }

    // if you kludge long enough, it becomes the standard
    // and no one remembers the before time
    public enum ControllerButton
    {
        // ... shapes?
        AButton,
        BButton,
        XButton,
        YButton,

        // D-Pad
        //Up,
        //Down,
        //Left,
        //Right,

        // bumpers
        RightBumper,
        LeftBumper,

        // other
        Start,
        Select,
    }

    public class InputManager : ISessionManager
    {
        public InputMonitor Monitor { get; private set; }
        private StandaloneInputModule _module;

        public Platform Platform { get; private set; }
        public Controller Controller { get; private set; }

        public Dictionary<ControllerButton, KeyCode> ButtonNames { get; private set; }

        public void Initialize () {
            SetupPlatform();
            SetupController();
            SetButtonNames();
            SetupInputModule();

            Monitor = Game.GO.AddComponent<InputMonitor>();
            Monitor.Initialize();
        }

        public void ShutDown () {
            Object.Destroy(Monitor);
            Monitor = null;
        }

        public void OnGameStart () { Monitor.OnGameStart(); }
        public void OnGameStop () { Monitor.OnGameStop(); }

        private void SetupPlatform () {
            switch (Application.platform) {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    Platform = Platform.OSX;
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    Platform = Platform.Windows;
                    break;
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    Platform = Platform.Linux;
                    break;
                default:
                    Logging.Error("Unknown platform: " + Application.platform);
                    Platform = Platform.Invalid;
                    break;
            }
        }

        private void SetupController () {
            var controllers = Input.GetJoystickNames();

            if (controllers.Length > 0) {
                if (controllers[0].Contains("Xbox")) {
                    Controller = Controller.XBox;
                    return;
                }

                if (controllers[0].Contains("Sony")) {
                    Controller = Controller.Dualshock;
                    return;
                }

                Logging.Error("Unknown controller name: " + controllers[0]);
            }

            Logging.Warn("No controllers plugged in.");
            Controller = Controller.None;
        }

        private void SetButtonNames () {
            if (Game.Sesh.Input.Controller == Controller.None) { return; }

            var xbox = Game.Sesh.Input.Controller == Controller.XBox;

            switch (Game.Sesh.Input.Platform) {
                case Platform.OSX:
                    ButtonNames = xbox ? ButtonMappings.OsxXBox : ButtonMappings.Osxds4;
                    break;
                case Platform.Windows: break;
                case Platform.Linux: break;
                case Platform.Invalid:
                    Logging.Error("Invalid platform; can't choose bindings.");
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupInputModule () {
            _module = Game.GO.GetRequiredComponentInChildren<StandaloneInputModule>();

            KeyCode buttonname;
            if (!ButtonNames.TryGetValue(ControllerButton.AButton, out buttonname)) {
                Logging.Error("Button names not set up?");
                return;
            }

            _module.submitButton = buttonname.ToString();
        }
    }

    // I am sorry about all the hard-coding
    public static class ButtonMappings
    {
        //public static readonly Dictionary<ControllerButton, string> WindowsXBox = new Dictionary<ControllerButton, string> { };
        //public static readonly Dictionary<ControllerButton, string> WindowsDS4 = new Dictionary<ControllerButton, string> { };
        public static readonly Dictionary<ControllerButton, KeyCode> OsxXBox =
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

        public static readonly Dictionary<ControllerButton, KeyCode> Osxds4 =
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