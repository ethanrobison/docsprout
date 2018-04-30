using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class HackRestart : MonoBehaviour
    {
        /// <summary>
        /// i am hack. beep boop.
        /// </summary>
        private bool _startPressed;

        private bool _selectPressed;

        private void Start () {
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.Start, () => { _startPressed = true; });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.Start, () => { _startPressed = false; },
                PressType.ButtonUp);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.Select, () => { _selectPressed = true; });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.Select, () => { _selectPressed = false; },
                PressType.ButtonUp);
        }

        private void Update () {
            if (_startPressed && _selectPressed) Game.Sesh.ReturnToMenu();
        }
    }
}